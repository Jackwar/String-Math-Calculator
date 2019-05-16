var isMobile = false;

function activateTextArea() {
   
    if (window.matchMedia("(min-width: 576px)").matches) {
        var input = document.getElementById("calculator-textbox");
        input.readOnly = false;
    }
    else {
        var calculatorDiv = document.getElementById("calculator-div");
        calculatorDiv.classList.add("sticky-top");
        isMobile = true;
    }
}

function insertText(text) {
    var input = document.getElementById("calculator-textbox");
    var event = new Event('change');
    var innerText = input.value;
    var cursorPos = innerText.length;
    if (!isMobile) {
        cursorPos = input.selectionStart;
    }
    var textBefore = innerText.substring(0, cursorPos);
    var textAfter = innerText.substring(cursorPos, innerText.length);

    input.value = textBefore + text + textAfter;
    input.dispatchEvent(event);
    input.focus();
    input.setSelectionRange(cursorPos + 1, cursorPos + 1);
}

function deleteText() {
    var input = document.getElementById("calculator-textbox");
    var cursorPos = input.value.length;
    if (!isMobile) {
        cursorPos = input.selectionStart;
    }

    if (cursorPos != 0) {
        var event = new Event('change');
        var innerText = input.value;
        var deletedText = innerText.slice(0, cursorPos - 1) + innerText.slice(cursorPos);

        input.value = deletedText;
        input.dispatchEvent(event);
        input.focus();
        input.setSelectionRange(cursorPos - 1, cursorPos - 1);
    }
}

function reFocusText() {
    var input = document.getElementById("calculator-textbox");
    input.focus();
}