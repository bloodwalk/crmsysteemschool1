$(document).ready(function () {
   // alert("het");
   // alert("jquery works");
    $.datepicker.regional['nl'] = {
        clearText: 'Effacer', clearStatus: '',
        closeText: 'sluiten', closeStatus: 'Onveranderd sluiten ',
        prevText: '<vorige', prevStatus: 'Zie de vorige maand',
        nextText: 'volgende>', nextStatus: 'Zie de volgende maand',
        currentText: 'Huidige', currentStatus: 'Bekijk de huidige maand',
        monthNames: ['januari', 'februari', 'maart', 'april', 'mei', 'juni',
            'juli', 'augustus', 'september', 'oktober', 'november', 'december'],
        monthNamesShort: ['jan', 'feb', 'mrt', 'apr', 'mei', 'jun',
            'jul', 'aug', 'sep', 'okt', 'nov', 'dec'],
        monthStatus: 'Bekijk een andere maand', yearStatus: 'Bekijk nog een jaar',
        weekHeader: 'Sm', weekStatus: '',
        dayNames: ['zondag', 'maandag', 'dinsdag', 'woensdag', 'donderdag', 'vrijdag', 'zaterdag'],
        dayNamesShort: ['zo', 'ma', 'di', 'wo', 'do', 'vr', 'za'],
        dayNamesMin: ['zo', 'ma', 'di', 'wo', 'do', 'vr', 'za'],
        dayStatus: 'Gebruik DD als de eerste dag van de week', dateStatus: 'Kies DD, MM d',
        dateFormat: 'dd/mm/yy', firstDay: 1,
        initStatus: 'Kies een datum', isRTL: false
    };
    $.datepicker.setDefaults($.datepicker.regional['nl']);
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
    //alert("also works");
    // hier begint keyup 
    $("#searchbox").keyup(function () {
        
        $("#EditTable tr").each(test);


        var VisibleRows = $('#EditTable tr:visible');
        //console.log(VisibleRows.length);
        VisibleRows.each(function (index) {
            
            if (index % 2 == 0 && index != 0) {
                console.log(index);
                $(this).css({ "background": "#D0E4F5" });
            }
            else if(index!=0) {
                $(this).css({ "background": "white" })
            }
        });
       
        }
            );
       


    // einde searchbox

var test = function test() {
    // alert("also works");
    var row = $(this);
    var columns = $(this).find('td').eq(0);
    columns.each(function () {
        var bool = false;
        var columnname = $(this).text();
        var searchvalue = $("#searchbox").val();
        // alert(columnname);			
        //alert(searchvalue);


        if (columnname.toLowerCase().startsWith(searchvalue.toLowerCase())) {

            var bool = true;
        }




        if (bool) {

            row.show();

        }
        else {
            row.hide();
        }


    });
};
   });

    // hier eindigd keyup




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