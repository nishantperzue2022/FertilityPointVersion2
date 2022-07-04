
function ShowLoader() {

    $("#loadMe").modal('show');
}

function HideLoader() {

    $("#loadMe").modal('hide');
}

$("#btnSubmitService").click(function () {



    if ($('#txtName').val() == '') {
        $('#txtName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Name is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtAmount').val() == '') {
        $('#txtAmount').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Amount is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    $("#modaService").modal('hide');

    var data = $("#frmServices").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/Services/Create/",

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



    if ($('#txtName1').val() == '') {
        $('#txtName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Name is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    if ($('#txtAmount1').val() == '') {
        $('#txtAmount1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Amount is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    $("#modaServiceUpdate").modal('hide');

    var data = $("#frmEditService").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/Services/Update/",

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













function GetService(e) {

    var id = e;

    console.log(id);

    $.get("/Admin/Services/GetById/?Id=" + id, function (data, status) {
        console.log(data);
        if (data.data == false) {
            alert("Does not exist");
        } else {

            $("#txtId").val(data.data.id);
            $("#txtName1").val(data.data.name);
            $("#txtAmount1").val(data.data.amount);



            $('#modaServiceUpdate').modal({ backdrop: 'static', keyboard: false })

            $("#modaServiceUpdate").modal('show');

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

                url: "/Admin/Services/Delete/" + id,

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