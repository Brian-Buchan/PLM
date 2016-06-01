var r1 = 0;
var g1 = 0;

var r2 = 0;
var g2 = 0;

var r3 = 0;
var g3 = 0;

function outputUpdate(val, selector) {
    document.querySelector(selector).value = val;
}

function validate() {
    //If all the settings are at their hardest values
    if (r1 === 255 && r2 === 255 && r3 === 255) {
        return confirm("Do you really want to do this to yourself?");
    }
    else {
        return confirm("Are you happy with these settings?")
    }
}
//this code was originally devised by nyloc8 and taken from http://jsfiddle.net/bataA/45/


$('input[type="range"]').each(function () {
    $(this).bind('mousedown', function () {
        $(this).bind('mousemove', function () {
            na = $('#numAnswers');
            nq = $('#numQuestions');
            nm = $('#time');

            //Number of Answers range slider
            r1 = ((na.val() - 3) / 12); //gets a ratio (0, 3, 6, 9, or 12 twelfths)
            r1 = (r1 * 255); //convert to a usable value
            r1 = Math.ceil(r1); //round up to get even steps

            g1 = (na.val() - 3); //sets value for green between 0 and 12
            g1 = ((6 - g1) + 6); //gets the value that's the opposite of the red number
            g1 = (g1 / 12); //gets the ratio for that number
            g1 = (g1 * 255); //convert to a useable value
            g1 = Math.ceil(g1); //round up

            na.css('box-shadow', 'inset 0 0 16px rgb(' + r1 + ',' + g1 + ',0)' +
                ',inset 16px 0 20px rgb(' + r1 + ',' + g1 + ',0)');

            //Number of Questions range slider
            r2 = (nq.val() - 5); //gets a number between 0 and 255, incremented by fives)

            g2 = (nq.val() - 5); //sets value for green between 0 and 255
            g2 = ((127 - g2) + 128); //gets the value that's the opposite of the red number 
                                     //(0 will give 255, 5 will give 250, so on and so forth)

            nq.css('box-shadow', 'inset 0 0 16px rgb(' + r2 + ',' + g2 + ',0)' +
                ',inset 16px 0 20px rgb(' + r2 + ',' + g2 + ',0)');

            //Time range slider
            r3 = (nm.val() - 15); //sets the value for red between 0 and 120
            r3 = ((60 - r3) + 60); //gets the value that's the opposite of the green number
            r3 = (r3 / 120); //gets the ratio for that number (reducible to 0-8 eigths)
            r3 = (r3 * 255); //convert to a useable value
            r3 = Math.ceil(r3); //round up

            g3 = ((nm.val() - 15) / 120); //gets a ratio (reducible to 0-8 eigths)
            g3 = (g3 * 255); //convert to a useable value
            g3 = Math.ceil(g3); //round up

            nm.css('box-shadow', 'inset 0 0 16px rgb(' + r3 + ',' + g3 + ',0)' +
                ',inset 16px 0 20px rgb(' + r3 + ',' + g3 + ',0)');
        });
    }).bind('mouseup', function () {
        $(this).unbind('mousemove');
    });
});

