$(document).ready(function () {
    // alert("het");
    // alert("jquery works");

    //$("#CallEditJS").click(function () {
    //    alert("The paragraph was clicked.");
    //    $(".datepicker").datepicker({ format: 'dd/mm/yyyy', autoclose: true, todayBtn: 'linked' })

    //}); 
   // $(".datepicker").datepicker({ format: 'dd/mm/yyyy', autoclose: true, todayBtn: 'linked' })

    // $("#SearchBox").css("background-color", "yellow"); pakt hem dus wel

    //var availableTags = [
    //    "ActionScript",
    //    "AppleScript",
    //    "Asp",
    //    "BASIC",];




    $("#SearchBox").autocomplete({
        // source: '@Url.Action("QuikSearchAsync", "Home")'
        source: function (request, response) {
            $.ajax(
                {
                    url: "/Home/QuikSearchAsync",
                    dataType: "json",
                    data:
                    {
                        term: request.term,
                    },
                    success: function (data) {
                        response(data);
                    }
                });
        }
        // source:availableTags
    })
});


var onSuccess = function (context) {
    // alert(context);
   
     $(".datepicker").datepicker({ format: 'dd/mm/yyyy', autoclose: true, todayBtn: 'linked' })
};

