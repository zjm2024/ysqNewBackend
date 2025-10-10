function addErrorMessage(message, element) {
    if (element) {
        element.closest('.control-group').removeClass('info').addClass('error');
        $("<span class=\"help-block\">" + message + "</span>").insertAfter(element);
    }
}

function removeErrorMessage(element) {
    if (element.next()) {
        element.closest('.control-group').removeClass('error');
        element.next().remove();
    }
}


function addErrorMessageReplace(message, element) {
    if (element) {
        element.closest('.control-group').removeClass('info').addClass('error');
        $("<span class=\"help-block\">" + message + "</span>").insertAfter(element);
    }
}

function removeErrorMessageReplace(element) {
    if (element.next()) {
        element.closest('.control-group').removeClass('error');
        element.next().remove();
    }
}

function addErrorMessageWithCss(message, element) {
    if (element) {
        element.parent().parent().addClass("has-error");
        element.closest('.control-group').removeClass('info').addClass('error');
        $("<span class=\"help-block\">" + message + "</span>").insertAfter(element);
    }
}

function removeErrorMessageWithCss(element) {
    if (element.next()) {
        element.parent().parent().removeClass("has-error");
        element.closest('.control-group').removeClass('error');
        element.next().remove();
    }
}