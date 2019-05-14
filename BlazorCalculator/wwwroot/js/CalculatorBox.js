var cursorPos = 0;

var replaceText = false;

function insertText(text) {
    var textBox = document.getElementById("calculator-textbox");

    if (!replaceText) {
        var cursorPos = textBox.selectionStart;
        var innerText = textBox.value;
        var textBefore = innerText.substring(0, cursorPos);
        var textAfter = innerText.substring(cursorPos, innerText.length);

        var event = new Event('change');

        textBox.value = textBefore + text + textAfter;
        textBox.dispatchEvent(event);
        textBox.focus();
        //textBox.selectionStart = cursorPos;
        textBox.setSelectionRange(cursorPos + 1, cursorPos + 1);
    }
    else {
        textBox.value = text;
        textBox.focus();
        textBox.setSelectionRange(1, 1);
        replaceText = false;
    }

}

function reFocusText() {
    var textBox = document.getElementById("calculator-textbox");

    replaceText = true;
}