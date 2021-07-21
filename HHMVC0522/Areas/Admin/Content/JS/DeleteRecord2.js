var URL = "";
var ID = "";
function AskQuestion(url, id,lastUrl) {
    $("#modalmessage").modal();
    URL = url;
    ID = id;
    LastUrl = lastUrl;
}
function Delete() {
    $.ajax(
        {
            url: URL + ID,            
            success: function () {
                console.log("成功")
                $("#a_" + ID).fadeOut();
                $("#modalmessage").modal('hide');
                window.location.href = "../" + LastUrl;
            },
            error: function () {
                console.log("錯誤")
            }
        })
}