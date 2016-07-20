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

            //The generic formula used to determine the number z opposite the number i, 
            //given number m is the midpoint between i and z is:
            //
            //(m - i) + m = z
            //
            //for example, we need to find the number opposite 17, if our range is 2-68.
            //The midpoint between these two numbers is (2 + 68)/2, or 35.
            //subtract 17 from 35 to get 18.
            //then add 35 to 18 to get 53.
            //thus, 53 is the number directly opposite 17 if 35 is the midpoint, 
            //as moving 18 steps in either direction on a number line will get you 17 or 53.
            //
            //to find the opposite number within any range, where
            //x = the minimum value of the range,
            //y = the maximum value of the range,
            //i = the inputted number,
            //and z is the output,
            //the formula is:
            //(((x + y) / 2) - i) + ((x + y) / 2) = z
            //
            //For these functions, I subtract a value from each value to let me use a zero-based range.
            //For example, the first range on the setup page, used to set Number of Answers, goes from 3 to 15.
            //This would be a pain to convert to a range from 0 to 255 (for color values), 
            //so before I do anything else, I subtract 3 from the given value for that slider, 
            //so that the range is now from 0 to 12 instead.
            //I then divide that number by 12 to get a ratio.
            //Multiplying that ratio by 255 gets me a useable color value proportional to the value of the range.

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
            //r3 = (nm.val() - 15); //sets the value for red between 0 and 120
            //r3 = ((60 - r3) + 60); //gets the value that's the opposite of the green number
            //r3 = (r3 / 120); //gets the ratio for that number (reducible to 0-8 eigths)
            //r3 = (r3 * 255); //convert to a useable value
            //r3 = Math.ceil(r3); //round up

            //g3 = ((nm.val() - 15) / 120); //gets a ratio (reducible to 0-8 eigths)
            //g3 = (g3 * 255); //convert to a useable value
            //g3 = Math.ceil(g3); //round up

            r3 = (nm.val() - 2); //sets the value for red between 0 and 30 (16 steps)
            r3 = ((15 - r3) + 15); //gets the value that's the opposite of the green number
            r3 = (r3 / 30); //gets the ratio for that number (fifteenths)
            r3 = (r3 * 255); //convert to a useable value
            r3 = Math.ceil(r3); //round up

            g3 = ((nm.val() - 2) / 30); //gets a ratio (fifteenths)
            g3 = (g3 * 255); //convert to a useable value
            g3 = Math.ceil(g3); //round up

            nm.css('box-shadow', 'inset 0 0 16px rgb(' + r3 + ',' + g3 + ',0)' +
                ',inset 16px 0 20px rgb(' + r3 + ',' + g3 + ',0)');
        });
    }).bind('mouseup', function () {
        $(this).unbind('mousemove');
    });
});

$('input[type="range"]').each(function () {
    $(this).bind('keydown', function () {
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
        //r3 = (nm.val() - 15); //sets the value for red between 0 and 120
        //r3 = ((60 - r3) + 60); //gets the value that's the opposite of the green number
        //r3 = (r3 / 120); //gets the ratio for that number (reducible to 0-8 eigths)
        //r3 = (r3 * 255); //convert to a useable value
        //r3 = Math.ceil(r3); //round up

        //g3 = ((nm.val() - 15) / 120); //gets a ratio (reducible to 0-8 eigths)
        //g3 = (g3 * 255); //convert to a useable value
        //g3 = Math.ceil(g3); //round up

        r3 = (nm.val() - 2); //sets the value for red between 0 and 30 (16 steps)
        r3 = ((15 - r3) + 15); //gets the value that's the opposite of the green number
        r3 = (r3 / 30); //gets the ratio for that number (fifteenths)
        r3 = (r3 * 255); //convert to a useable value
        r3 = Math.ceil(r3); //round up

        g3 = ((nm.val() - 2) / 30); //gets a ratio (fifteenths)
        g3 = (g3 * 255); //convert to a useable value
        g3 = Math.ceil(g3); //round up

        nm.css('box-shadow', 'inset 0 0 16px rgb(' + r3 + ',' + g3 + ',0)' +
            ',inset 16px 0 20px rgb(' + r3 + ',' + g3 + ',0)');

    });
});

