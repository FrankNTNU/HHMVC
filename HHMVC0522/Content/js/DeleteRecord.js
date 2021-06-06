var URL = "";
var ID = "";
$(document).ready(function () {
    function AskQuestion(url, id) {
        $("#modalmessage").modal();
        URL = url;
        ID = id;
    }
    function Delete() {
        $.ajax(
            {
                url: URL + ID,
                type: "POST",
                success: function () {
                    $("#a_" + ID).fadeOut();
                    $("#modalmessage").modal('hide');
                }
            })
    }
});
