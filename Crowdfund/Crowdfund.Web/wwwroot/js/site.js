// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let successAlert = $('.js-success-alert');
successAlert.hide();

let failAlert = $('.js-fail-alert');
failAlert.hide();

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

    projectid = $('#ProjectId');

    sendData = {
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": parseInt($('#Category').val())
    }

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