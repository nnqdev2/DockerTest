
function measureGroupType_OnChange() {
    var selectedmeasureGroupType = $('#MeasureGroupType_measureGroupType').val();                                                       //get the new value.
    var TypeSelect = $('#MeasureGroupType_SelectedType');                                                   //Then get the makes dropdown
    var GroupSelect = $('#MeasureGroupType_SelectedGroup');
    var Typelabel = $('label[for="MeasureGroupType_SelectedType"]');
    var GroupStatuslabel = $('label[for="Status"]');
    var StatusSelect = $('#Status'); //and disable the models dropdown.
    if (selectedmeasureGroupType == "Type") {
        $('#lblTitle').css('visibility', 'visible');
        $('#txtTitle').css('visibility', 'visible');
        $('#successMessage').hide();
        TypeSelect.hide();
        StatusSelect.hide();
        Typelabel.hide();
        GroupStatuslabel.hide();
        $('#MeasureGroupType_SelectedGroup').hide();
        $('label[for="MeasureGroupType_SelectedGroup"]').hide();
        $('#panelDescription').hide();
        $('#panelRange').hide();
        $('#panelOwner').hide();
        $('#panelRollUp').hide();
    }
    else if (selectedmeasureGroupType == "Group") {
        $('#titlevalidation').css('visibility', 'hidden');
        $('#lblTitle').css('visibility', 'visible');
        $('#txtTitle').css('visibility', 'visible');
        $('#MeasureGroupType_SelectedGroup').hide();
        $('label[for="MeasureGroupType_SelectedGroup"]').hide();
        $('#successMessage').hide();
        $('#panelDescription').hide();
        $('#panelRange').hide();
        $('#panelOwner').hide();
        TypeSelect.show();
        StatusSelect.show();
        Typelabel.show();
        GroupStatuslabel.show();
        $('#panelRollUp').hide();
    }
    else if (selectedmeasureGroupType == "Measure") {
        $('#titlevalidation').css('visibility', 'hidden');
        $('#lblTitle').css('visibility', 'visible');
        $('#txtTitle').css('visibility', 'visible');
        //$('#MeasureGroupType_SelectedType').css('visibility', 'hidden');
        //$('label[for="MeasureGroupType_SelectedType"]').css('visibility', 'hidden');
        $('#successMessage').show();
        TypeSelect.hide();
        StatusSelect.show();
        Typelabel.hide();
        GroupStatuslabel.show();
        $('#MeasureGroupType_SelectedGroup').show();
        $('label[for="MeasureGroupType_SelectedGroup"]').show();
        $('#panelDescription').show();
        $('#panelRange').show();
        $('#panelOwner').show();
        $('#panelRollUp').hide();
    }
    else if (selectedmeasureGroupType == "RollUp") {
        $('#titlevalidation').css('visibility', 'hidden');
        $('#lblTitle').css('visibility', 'visible');
        $('#txtTitle').css('visibility', 'visible');
        $('#successMessage').hide();
        //$('#lblTitle').css('visibility','hidden');
        //$('#txtTitle').css('visibility', 'hidden');
        TypeSelect.hide();
        StatusSelect.hide();
        Typelabel.hide();
        GroupStatuslabel.hide();
        $('#MeasureGroupType_SelectedGroup').show();
        $('label[for="MeasureGroupType_SelectedGroup"]').show();
        $('#panelDescription').hide();
        $('#panelRange').hide();
        $('#panelOwner').hide();
        //$('#panelRollUp').show();
       
    }
}
//function measureOwner_AutoComplete() {
// 
//    $('#MeasureOwner').autocomplete(
//        {
//        source: '@Url.Action("AutoCompleteMeasureOwner")'
//    });
//}
function measureGroup_OnChange() {  
    var selectedmeasureGroupType = $('#MeasureGroupType_measureGroupType').val();
    var GroupSelect = $('#MeasureGroupType_SelectedGroup').val();

    var measuresSelect = $('#MeasuresForSelectedGroup');

    measuresSelect.empty();
    if (GroupSelect == null || selectedmeasureGroupType=="Measure") {                                                               //If the value was changed back to nothing,
        measuresSelect.prop('disabled', 'disabled');
        TypeSelect.hide();
        Typelabel.hide();
        //disable the makes dropdown, too.
    }
    else {
        $('#panelRollUp').show();
        //$.ajax({
        //    url: "/Measures/MeasureByGroup",
        //    type: "GET",
        //    data: { Group: GroupSelect },
        //    traditional: true,
        //    success: function (measures) {
        //        $.each(measures, function (index, measure) {
        //            measuresSelect.append(
        //                $('<option></option>').attr('value',measure).html(measure)
        //            );
                    
        //        })
        //    },
        //    error: function () {
        //        alert("Something went wrong call the police");
        //    }
        //});
        //Otherwise,
        $.getJSON('/Measures/MeasureByGroup', { Group: GroupSelect }, function (measures) {  //get a list of makes for the year,
            measuresSelect.prop('disabled', false);  
            $.each(measures, function (index, measure) {
                
                $('#MeasuresForSelectedGroup').append(                                                         //append an option.
                    $('<option/>')
                        .attr('value', measure)
                        .text(measure)
                );
            });
        });
       
    }
}
function validation_check() {
    
    var selectedmeasureGroupType = $('#MeasureGroupType_measureGroupType').val(); 
    
    if (selectedmeasureGroupType == "Type")
    {
        if ($('#txtTitle').val() == "")
        {
            txtTitle.classList.add('error');
            alert('Please complete the title field');
        }
    }
    else if (selectedmeasureGroupType == "Group")
    {
        if ($('#txtTitle').val() == "") {
            $('#titlevalidation').css('visibility', 'visible');
        }
    }
    else if (selectedmeasureGroupType == "Measure")
    {
        if ($('#txtTitle').val() =="") {
            $('#titlevalidation').css('visibility', 'visible');
        }
        if ($('#Description').val() == "") {
            $('#Descriptionvalidation').css('visibility', 'visible');
        }
        if ($('#txtTarget').val() == "") {
            $('#Targetvalidation').css('visibility', 'visible');
        }
        if ($('#txtGreenRange').val() == "") {
            $('#GreenRangevalidation').css('visibility', 'visible');
        }
        if ($('#txtRedRange').val() == "") {
            $('#RedRangevalidation').css('visibility', 'visible');
        }
    }
    else if (selectedmeasureGroupType == "RollUp") {

    }


}