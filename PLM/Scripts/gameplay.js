$(function () {
    //Sounds from http://www.freesfx.co.uk/
    var pictureAnswer = "default";
    var count = Number($('#displayScore').text());
    var revealed = false;
    var intervalID;

    function windowOnload(timeLeft) {
        CheckMute();
        startCountdown(timeLeft);
    }
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

    function ButtonClick(guess) {
        pictureAnswer = $("#StoredAnswer").text();
        if (!revealed) {
            if (isGuessRight(pictureAnswer, guess)) {
                Correct();
            }
        }
    }

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

    function showNext() {
        document.getElementById("nextButton").style.display = "inline";
    }

    function Correct() {
        count = (parseInt(count, 10) + 100);
        //check the unchecked radio button (set "Correct" to true)
        $('input[type="radio"]').not(':checked').prop("checked", true);
        $('#Score').val(count);
        $('#displayScore').text(count);
    }

    //This image only implementation of a mute button is from Tarun at 
    //http://stackoverflow.com/questions/22918220/how-to-create-a-only-mute-unmute-button-like-youtube-in-html
    //Toggle whether sound will play
    function ToggleMute(img) {
        //if the muteSound cookie is set to true
        if (getCookie("muteSound") === "true") {
            //set the cookie to false and display the speaker symbol
            setCookie("muteSound", "false", 365);
            img.src = '/PerceptualLearning/Content/Images/speaker.png';
        }
        else {
            //otherwise, set muteSound to true and display the mute symbol
            setCookie("muteSound", "true", 365);
            img.src = '/PerceptualLearning/Content/Images/mute.png';
        }
    }

    function CheckMute() {
        if (getCookie("muteSound") === "true") {
            document.getElementById('soundToggle').setAttribute('src', '/PerceptualLearning/Content/Images/mute.png');
        } else {
            document.getElementById('soundToggle').setAttribute('src', '/PerceptualLearning/Content/Images/speaker.png');
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

    function fixAspect(img) {
        if (img.height > img.width) {
            img.height = '100%';
            img.width = 'auto';
        }
    }
});