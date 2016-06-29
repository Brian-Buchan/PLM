//Sounds from http://www.freesfx.co.uk/
var pictureAnswer = "default";
var count = Number(document.getElementById("displayScore").innerText);
var revealed = false;
var intervalID;

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

//Check if the user's guess is right
//answer: the correct answer, from #StoredAnswer
//guess: the user's guess (expects string)
function isGuessRight(answer, guess) {
    if (answer == guess) {
        if (getCookie("muteSound") !== "true") {
            document.getElementById("audioCorrect").play();
        }
        $("#resultText").text("Yes, the correct answer is " + answer);
        reveal();
        showNext();
        return true;
    }
    else {
        if (getCookie("muteSound") !== "true") {
            document.getElementById("audioIncorrect").play();
        }
        $("#resultText").text("Sorry, the correct answer was actually " + answer);
        reveal();
        showNext();
        return false;
    }
}

function ButtonClick(guess) {
    pictureAnswer = $("#StoredAnswer").text();
    if (!revealed) {
        if (isGuessRight(pictureAnswer, guess)) {
            Correct();
        }
        else {

        }
    }
}

function showNext() {
    document.getElementById("nextButton").style.display = "inline";
}

function reveal() {
    pictureAnswer = $("#StoredAnswer").text();

    $('.btn').each(function () {
        $(this).disabled = true;
        if ($(this).text() === pictureAnswer) {
            $(this).css({ "background-color": "green" });
        } else {
            $(this).css({ "background-color": "red" });
        }

    });
    revealed = true;
}

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

function CheckIn() {
    //Stop the timer when you click the "Next" button.
    clearInterval(intervalID);
    //Then get the time remaining.
    $('#Time').val($('#clockdiv').text());
    return true;
}

function startCountdown(time) {
    var dur = moment.duration(time);
    //Global variable intervalID is used to index the interval
    intervalID = setInterval(function () {
        //subtract a single second
        dur = dur.subtract(1, 's');
        //forces a leading zero if there is only one digit in the time by 
        //taking the last two characters of a string: 
        //"0" + "1" becomes "01", while "0" + "12" becomes "12"
        $('#clockdiv').text(
            ('0' + dur.hours()).slice(-2) + ':'
            + ('0' + dur.minutes()).slice(-2) + ':'
            + ('0' + dur.seconds()).slice(-2)
            );
        //If time is up
        if (dur.hours() == 0 & dur.minutes() == 0 & dur.seconds() == 0) {
            //stop the timer
            clearInterval(intervalID);
            //update the data
            CheckIn();
            //submit the form
            document.getElementById("GameForm").submit();
        }
    }, 1000);
}