$(document).ready(function () {
   // alert("het");
   // alert("jquery works");
    $(".datepicker").datepicker({ format: 'dd/mm/yyyy', autoclose: true, todayBtn: 'linked' })

   // $("#SearchBox").css("background-color", "yellow"); pakt hem dus wel

    //var availableTags = [
    //    "ActionScript",
    //    "AppleScript",
    //    "Asp",
    //    "BASIC",];




    $("#SearchBox").autocomplete({
       // source: '@Url.Action("QuikSearchAsync", "Home")'
       source: function(request, response) {
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

    // hier begint alle notities/////////////////////////////////////////////////////////////////////////
    
    $("#crmbutton2").on("click", function () {


        switch ($(this).html()) {

            case "Show All":


                $(".shownotities").each(function (index) {

                    if ($(this).is(":hidden")) {

                        $(".shownotities").eq(index).toggle();
                        $(".caret2").eq(index).toggle();
                    }

                });

                $("#crmbutton2").html("Hide All");


                break;

            case "Hide All":


                $(".shownotities").each(function (index) {

                    if ($(this).is(":visible")) {

                        $(".shownotities").eq(index).toggle();
                        $(".caret2").eq(index).toggle();
                    }

                });

                $("#crmbutton2").html("Show All");
                break;



        }
    });

    $(".crmbutton").each(function (index) {

        $(this).on("click", function () {
               
            $(".shownotities").eq(index).toggle();
            $(".caret2").eq(index).toggle();

            if (IsAllOpen()) {
               
                $("#crmbutton2").html("Hide All");

            }
            else if (IsAllClosed()) {

                $("#crmbutton2").html("Show All");
            }

        });


    });

    function IsAllClosed() {
        var result = true;
        $(".crmbutton").each(function (index) {
           
            if ($(".shownotities").eq(index).is(":visible")) {
                result = false;
                return false;
            }
        });

        return result;



    }

    function IsAllOpen() {
        var result = true;
        $(".crmbutton").each(function (index) {

            if ($(".shownotities").eq(index).is(":hidden")) {
                result = false;
                return false;
            };

        });

        return result;


    }    
    
    //hier eindigd alle notities
    
});



    //$("#EditSubmit2").click(function () {
    //    alert("Handler for .click() called.");
    //});
    
    //$("input[data-autocomplete-source]").each(function () {

    //    var target = $(this);
    //    target.autocomplete({ source: target.attr("data-autocomplete-source") });
    //})
     ///////////////////////////////////////////////////// hierboven misschien later nog gebruiken
    //$("#EditSubmit").click(function () {


    //   // alert("clicked"); //werkt wel

    //    $.ajax({
    //        type: 'GET',
    //        url: "/Home/Demo1",
    //        success: function (result) {
    //            $("#EditData").html(result);
    //        }
    //    });
    //});


    //niewe code
    //$("#EditForm").submit(function (e) {

    //    alert("clicked");
    //    var form = $(this);

    //    $.ajax({
    //        type: 'GET',
    //        url: "/Home/Demo1",
    //        data: form.serialize(),
    //        success: function (result) {
    //            $("#EditData").html(result);
    //        }
    //    });

    //    e.preventDefault();

    //}







    ////////////////////////////////////////// voor morgen.
    //$("#idForm").submit(function (e) {


    //    var form = $(this);
    //    var url = form.attr('action');

    //    $.ajax({
    //        type: "POST",
    //        url: url,
    //        data: form.serialize(), // serializes the form's elements.
    //        success: function (data) {
    //            alert(data); // show response from the php script.
    //        }
    //    });

    //    e.preventDefault(); // avoid to execute the actual submit of the form.
    //});







    //document.getElementById("para").onclick = function () { alert("hey!")};

    //$(".datepicker").click(function () {
    //    $(this).hide();
    //}); datepicker en jquery werken allebei gewoon

var onSuccess = function (context) {
    // alert(context);

    $(".datepicker").datepicker({ format: 'dd/mm/yyyy', autoclose: true, todayBtn: 'linked' })
};

//function Success() {

//    //$('#divloading').hide();

//    //data - ajax - success="Success"data - ajax - failure="Failure"
//}

//function Failure() {

//    //$('#divloading').hide();
//    //alert("Form Failed");
//}