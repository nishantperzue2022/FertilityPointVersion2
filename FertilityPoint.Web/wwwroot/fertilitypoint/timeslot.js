$(document).ready(function () {
    GetSlots();
});


function GetSlots() {
       

    var t = $('#tblSlots').DataTable({

        "ajax": {
            "url": "/Admin/TimeSlots/AjaxMethod",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [

            { 'data': 'id' },
            { 'data': 'newFromTime' },
            { 'data': 'newToTime' },
            { 'data': 'newAppointmentDate' },

            {
                data: null,

                mRender: function (data, type, row) {

                    var status = row.isBooked;

                    if (status == 0) {

                        return "<span class='activeUser'> Available </span>"
                    }
                    else {
                        return "<span class='disabledUser'> Booked </span>"
                    }


                }
            },

            { 'data': 'newCreateDate' },

            {
                data: null,
                mRender: function (data, type, row) {
                    return "<a href='#' class='btn btn-success btn-sm '    onclick=FetchCarDetails('" + row.id + "'); >Edit </a> / <a href='#' class='btn-sm btn-danger'   onclick=DeleteRecord('" + row.id + "'); >Delete</a>";

                }
            }



        ]

    });
    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}




function ShowLoader() {

    $("#loadMe").modal('show');
}

function HideLoader() {

    $("#loadMe").modal('hide');
}

$("#btnSubmit").click(function () {



    if ($('#txtTime').val() == '') {
        $().focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Time is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    $("#modalTimeSlot").modal('hide');

    var data = $("#frmTimeSlot").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/TimeSlots/Create/",

        data: data,

        beforeSend: function () { ShowLoader(); },

        success: function (response) {

            if (response.success) {


                swal({

                    position: 'top-end',

                    type: "success",

                    title: response.responseText,

                    showConfirmButton: false,

                }), setTimeout(function () { location.reload(); }, 3000);

            } else {

                swal({

                    position: 'top-end',

                    type: "error",

                    title: response.responseText,

                    showConfirmButton: true,

                    timer: 5000,
                });


            }
        },

        error: function (response) {
            alert("error!");
        },
        complete: function () {
            HideLoader();
        }
    })

})


$("#btnEdit").click(function () {



    if ($('#txtTime1').val() == '') {

        $('#txtTime1').focus();

        swal({
            position: 'top-end',
            type: "error",
            title: "Time is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    $("#modalTimeSlotUpdate").modal('hide');

    var data = $("#frmEditTimeSlot").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/TimeSlots/Update/",

        data: data,

        beforeSend: function () { ShowLoader(); },

        success: function (response) {

            if (response.success) {


                swal({

                    position: 'top-end',

                    type: "success",

                    title: response.responseText,

                    showConfirmButton: false,

                }), setTimeout(function () { location.reload(); }, 3000);

            } else {

                swal({

                    position: 'top-end',

                    type: "error",

                    title: response.responseText,

                    showConfirmButton: true,

                    timer: 5000,
                });


            }
        },

        error: function (response) {
            alert("error!");
        },
        complete: function () {
            HideLoader();
        }
    })

})

function GetTimeSlot(e) {

    var id = e;

    console.log(id);

    $.get("/Admin/TimeSlots/GetById/?Id=" + id, function (data, status) {
        console.log(data);
        if (data.data == false) {
            alert("Does not exist");
        } else {

            $("#txtId").val(data.data.id);
            $("#txtFromTime1").val(data.data.newFromTime);
            $("#txtToTime1").val(data.data.newToTime);

            $('#modalTimeSlotUpdate').modal({ backdrop: 'static', keyboard: false })

            $("#modalTimeSlotUpdate").modal('show');
        }

    });
};

function DeleteRecord(e) {

    var id = e;

    swal(

        {
            title: "Are you sure?",

            //text: "Deac!",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $.ajax({

                type: "GET",

                url: "/Admin/TimeSlots/Delete/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 3000);

                    }

                    else {
                        swal({
                            position: 'top-end',
                            type: "error",
                            title: response.responseText,
                            showConfirmButton: true,
                            timer: 5000,
                        });

                    }

                },
                error: function (response) {

                    console.log(response);
                    swal({
                        position: 'top-end',
                        type: "error",
                        title: "Server error ,kindly contact the admin for assistance",
                        showConfirmButton: false,
                        timer: 5000,
                    });

                }

            })

        });
}

