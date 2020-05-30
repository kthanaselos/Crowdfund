// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let successAlert = $('.js-success-alert');
successAlert.hide();

let failAlert = $('.js-fail-alert');
failAlert.hide();

let projectSuccessAlert = $('.js-project-success-alert');
projectSuccessAlert.hide();

let projectFailAlert = $('.js-project-fail-alert');
projectFailAlert.hide();

$('.new-project-form-packages').hide();

function editUser() {
    successAlert.hide();
    failAlert.hide();

    userid = $('#UserId');

    sendData = {
        "FirstName": $('#FirstName').val(),
        "LastName": $('#LastName').val(),
        "Email": $('#Email').val()
    }

    $.ajax({
        type: 'PATCH',
        url: `/user/${userid.text()}`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(user => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function submitNewUser() {
    successAlert.hide();
    failAlert.hide();

    sendData = {
        "FirstName": $('#FirstName').val(),
        "LastName": $('#LastName').val(),
        "Email": $('#Email').val()
    }

    $.ajax({
        type: 'POST',
        url: `/user/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(user => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function editProject() {
    successAlert.hide();
    failAlert.hide();

    let mediaUrl = [$('#ImageURL1').val(),
            $('#ImageURL2').val(),
            $('#ImageURL3').val(),
        $('#VideoURL').val()];

    projectid = $('#ProjectId');
    debugger;
    sendData = {
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": parseInt($('#Category').val()),
        "MediaURLs": mediaUrl
    }
    debugger;
    alert(JSON.stringify(sendData));
    debugger;
    $.ajax({
        type: 'PATCH',
        url: `/project/${projectid.text()}`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function createProject() {
    successAlert.hide();
    failAlert.hide();

    let mediaUrl = [$('#ImageURL1').val(),
        $('#ImageURL2').val(),
        $('#ImageURL3').val(),
        $('#VideoURL').val()];

    sendData = {
        "UserId": parseInt($('#UserId').val()),
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": parseInt($('#Category').val()),
        "MediaURLs": mediaUrl,
        "FinancialGoal": parseFloat($('#FinancialGoal').val())
    }
    alert(JSON.stringify(sendData));
    debugger;
    $.ajax({
        type: 'POST',
        url: `/project/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        debugger;
        successAlert.show().delay(1000);
        successAlert.fadeOut(2000);
        $('.new-project-form').hide();
        $('#ProjectId').val(project.projectId);
        $('.new-project-form-packages').show();
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function addPackageToProject() {
    successAlert.hide();
    failAlert.hide();

    sendData = {
        "ProjectId": parseInt($('#ProjectId').val()),
        "Description": $('#Description').val(),
        "Price": parseFloat($('#Price').val())
    }
    alert(JSON.stringify(sendData));
    debugger;
    $.ajax({
        type: 'POST',
        url: `/package/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        debugger;
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}