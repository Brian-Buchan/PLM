// Hello.
//
// This is JSHint, a tool that helps to detect errors and potential
// problems in your JavaScript code.
//
// To start, simply enter some JavaScript anywhere on this page. Your
// report will appear on the right side.
//
// Additionally, you can toggle specific options in the Configure
// menu.

//Sounds from http://www.freesfx.co.uk/
var pictureAnswer = "default";
var count = Number(document.getElementById("displayScore").innerText);

//These cookie functions are from w3schools
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) === 0) return c.substring(name.length, c.length);
    }
    return "";
}

function isGuessRight(answer, guess) {
    if (answer == guess) {
        if (getCookie("muteSound") !== "true") {
            document.getElementById("audioCorrect").play();
        }
        $("#resultText").text("Indubitably, the correct answer is in fact " + answer);
        reveal();
        showNext();
        return true;
    }
    else {
        if (getCookie("muteSound") !== "true") {
            document.getElementById("audioIncorrect").play();
        }
        $("#resultText").text("Preposterous, the correct answer was actually " + answer);
        reveal();
        showNext();
        return false;
    }
}
//Still being worked on
function ButtonClick(guess) {
    pictureAnswer = $("#StoredAnswer").text();

    if (isGuessRight(pictureAnswer, guess)) {
        Correct();
    }
    else {

    }
}

//legacy function
//function ButtonClick(guess) {
//    pictureAnswer = document.getElementById("StoredAnswer").innerText;

//    if (isGuessRight(pictureAnswer, guess)) {
//        Correct();
//    }
//    else {

//    }
//}

function showNext() {
    document.getElementById("nextButton").style.display = "inline";
}

//Currently working on function
function reveal() {
    pictureAnswer = $("#StoredAnswer").text();

    $('.btn').each(function () {
        if ($(this).text() === pictureAnswer) {
            $(this).css({ "background-color": "green" });
        } else {
            $(this).css({ "background-color": "red" });
        }
    });
}

//Legacy function
//function reveal() {
//    pictureAnswer = document.getElementById("StoredAnswer").innerText;

//    $('.btn').each(function () {
//        if (this.innerText == pictureAnswer) {
//            this.style.backgroundColor = "green";
//        } else {
//            this.style.backgroundColor = "red";
//        }
//    });
//}

function Correct() {
    count = (parseInt(count) + 100);
    document.getElementById("Score").value = count;
    document.getElementById("displayScore").innerText = count;
}

//Toggle whether sound will play
function ToggleMute() {
    //if the muteSound cookie is set to true
    if (getCookie("muteSound") === "true") {
        //set the cookie to false
        setCookie("muteSound", "false", 365);
    }
    else {
        //otherwise, set muteSound to true
        setCookie("muteSound", "true", 365);
    }
}