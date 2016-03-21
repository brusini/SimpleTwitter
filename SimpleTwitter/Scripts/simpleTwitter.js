var Manager = {
    formatDate: function (date) {
        var d = date == null ? new Date() : new Date(date);
        return $.datepicker.formatDate("M d, yy", d) + " " + d.getHours() + ":" + d.getMinutes();
    }
};

$(function () {

    var userName = null, tips = $(".validateTips"),
        txtUserName = $("#userName"), skip = 0, reachedEnd = false;;

    var dialog = $("#dialog-form").dialog({
        autoOpen: true,
        width: 470,
        height:200,
        modal: true,
        buttons: {
            "Login": doLogin
        }
    });

    function doLogin() {
        var valid = true;
        var allFields = $([]).add(txtUserName);
        allFields.removeClass("ui-state-error");

        valid = valid && checkLength(txtUserName, "userName", 3, 16);
        if (valid) {
            userName = txtUserName.val();
            dialog.dialog("close");
            $("#btnPost").attr('disabled', false);
            $(".btnComment").attr('disabled', false);
            $('.userName').html("&#64;" + htmlEncode(userName));


            $('.userName').data("loggedId", true);
            $('#txtMsg').focus();
        }
        return valid;
    };

    function getMessages() {
        var data = {
            take: 15,
            skip: skip
        };
        console.log(data);
        $.ajax("/api/messages", {
            data: data,
            dataType: "json",
            type: "GET",
            beforeSend: function (xhr) {
                $("#results").after($("<p class='loading'>Loading...</p>").fadeIn('slow')).data("loading", true);
            },
            success: function (data) {
                $(".loading").fadeOut('fast', function () {
                    $(this).remove();
                });
                if (data.length == 0) reachedEnd = true;
                $("#postTemplate").tmpl(data).appendTo("#results");
                $("#results").removeData("loading");
            },
            error: function (e, s) {
                console.log(e);
            }
        });
        skip += 10;
    };

    function getComments(messageId) {
        $.ajax("/api/comments", {
            data: {
                messageId: messageId
            },
            dataType: "json",
            type: "GET",
            success: function (data) {
                $("#commentTemplate").tmpl(data).appendTo('#comments_' + messageId);
                $('#comments_' + messageId).removeClass('hidden');
                $('#showComments_' + messageId).addClass('hidden');
                $('#comment-form_' + messageId).removeClass('hidden');
            },
            error: function (e, s) {
                console.log(e);
            }
        });
    };

    function addMessage() {
        var text = $('#txtMsg').val();
        $.ajax("/api/messages/add", {
            data: {
                UserName: userName,
                Message: text
            },
            dataType: "json",
            type: "PUT",
            success: function (id) {
                $('.validation-summary').addClass('hidden');
                $('.validation-summary').html('');
                var d = new Date();
                var data = [{
                    Id: id,
                    TextMessage: text.trim(),
                    UserName: userName,
                    DatePosted: Manager.formatDate()
                }];

                $("#postTemplate").tmpl(data).prependTo("#results");
                $('#txtMsg').val('');
                $('#txtMsg').focus();
            },
            error: function (e, s) {
                console.log(e);
                $('.validation-summary').removeClass('hidden');

                var messages = populateErrors(e.responseJSON);
                $('.validation-summary').html(messages);
            }
        });
    };

    function addComment(id) {
        var text = $('#txtComment_' + id).val();
        $.ajax("/api/comments/add", {
            data: {
                MessageId: id,
                UserName: userName,
                Message: text
            },
            dataType: "json",
            type: "PUT",
            success: function () {
                $('.validation-summary_' + id).addClass('hidden');
                $('.validation-summary_' + id).html('');
                var d = new Date();
                var data = [{
                    TextMessage: text.trim(),
                    UserName: userName,
                    DatePosted: Manager.formatDate()
                }];
                $('#txtComment_' + id).val('');
                $("#commentTemplate").tmpl(data).prependTo("#comments_" + id);
            },
            error: function (e, s) {
                console.log(e);
                $('.validation-summary_' + id).removeClass('hidden');

                var messages = populateErrors(e.responseJSON);
                $('.validation-summary_' + id).html(messages);
            }
        });
    };

    function populateErrors(responseJSON) {
        var message = responseJSON.ExceptionMessage;
        if (responseJSON.ModelState != null) {
            $.each(responseJSON.ModelState, function (i, fieldItem) {
                message += fieldItem;
                message += "\n";
            });
        }
        return message;
    }

    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    function updateTips(t) {
        tips
        .text(t)
        .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " +
                min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    };

    $(window).on('scroll', function () {
        if (reachedEnd) return;
        var $results = $("#results");
        var wintop = $(window).scrollTop(), docheight = $(document).height(), winheight = $(window).height();
        var scrolltrigger = 0.99;

        if ((wintop / (docheight - winheight)) > scrolltrigger) {
            if (!$results.data("loading")) {
                getMessages();
            }
        }
    }).scroll();

    $('body').on('click', '.showComments', function () {
        var id = $(this).attr('msgId');
        if ($('.userName').data("loggedId") == true) {
            $(".btnComment").attr('disabled', false);
        }
        getComments(id);
    });

    $('body').on('submit', '.comment-form', function () {
        var id = $(this).attr('msgId');
        addComment(id);
        event.preventDefault();
    });

    $("#post-form").submit(function (event) {
        addMessage();
        event.preventDefault();
    });

    $("#dialog-form").submit(function (event) {
        doLogin();
        event.preventDefault();
    });

    dialog.dialog("open");
    getMessages();
});