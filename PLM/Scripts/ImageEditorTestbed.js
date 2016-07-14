var imgEdit = document.getElementById("3");
var imgIdInput = document.getElementById("imgId");
var answerIdInput = document.getElementById("answerId");
var imgDataInput = document.getElementById("imgData");
var origUrlInput = document.getElementById("origUrl");
var tempUrlInput = document.getElementById("tempUrl");

var myPixie = Pixie.setOptions({
    replaceOriginal: true,
    appendTo: 'body',
    onSave: function (data, img) {
        $.ajax({
            type: 'POST',
            url: '/ImageEditor/ImageEditor',
            data: { imgData: data, imgId: img.id, answerId: img.title, origUrl: img.dataset.origSource },
            success: function (xhr, status, text) {
                imgIdInput.value = imgEdit.id;
                answerIdInput.value = imgEdit.title;
                imgDataInput.value = imgEdit.src;
                origUrlInput.value = imgEdit.dataset.origSource;
                tempUrlInput.value = text.statusText;

                alert('Image changes have been saved successfully. \nRemember to ' +
                'click the save button to finalize your changes.');
                myPixie.close();
            },
            error: function (xhr, status, text) {
                alert(text);
                myPixie.close();
            }
        });
    }
});

$('#3').on('click', function (e) {
    myPixie.open({
        url: e.target.src,
        image: e.target
    });
});