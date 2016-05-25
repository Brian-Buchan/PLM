function outputUpdate(val, selector) {
    document.querySelector(selector).value = val;
}

//this code was originally devised by nyloc8 and taken from http://jsfiddle.net/bataA/45/

var r1 = 0;
var g1 = 0;

var r2 = 0;
var g2 = 0;

$('input[type="range"]').each(function () {
    $(this).bind('mousedown', function () {
        $(this).bind('mousemove', function () {
            na = $('#numAnswers');
//          nq = $('#numQuestions');

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


//            r2 = ((na.val() - 3) / 12); //gets a ratio (0, 3, 6, 9, or 12 twelfths)
//            r2 = (r2 * 255); //convert to a usable value
//            r2 = Math.ceil(r2); //round up to get even steps

//            g2 = (na.val() - 3); //sets value for green between 0 and 12
//            g2 = ((6 - g2) + 6); //gets the value that's the opposite of the red number
//            g1 = (g1 / 12); //gets the ratio for that number
//            g1 = (g1 * 255); //convert to a useable value
//            g1 = Math.ceil(g1); //round up

//            na.css('box-shadow', 'inset 0 0 16px rgb(' + r1 + ',' + g1 + ',0)' +
//                ',inset 16px 0 20px rgb(' + r1 + ',' + g1 + ',0)');
        });
    }).bind('mouseup', function () {
        $(this).unbind('mousemove');
    });
});