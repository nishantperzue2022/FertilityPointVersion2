

function ShowLoader() {

    $("#loadMe").modal('show');
}

function HideLoader() {

    $("#loadMe").modal('hide');
}


function GetMessage(e) {


   
    $('#divReply').show();



    var id = e;

    console.log(id);

    $.get("/Admin/Enquiries/GetById/?Id=" + id, function (data, status) {
        console.log(data);
        if (data.data == false) {
            alert("Does not exist");
        } else {

            $("#txtId").val(data.data.id);
            $("#txtMessage").text(data.data.message);
            $("#txtPhoneNumber").text(data.data.phoneNumber);
            $("#txtEmail").text(data.data.email);
            $("#txtStatus").text(data.data.status);
            $("#txtName").text(data.data.name);
        }

    });
};


$("#btnReply").click(function () {



    if ($('#txtComposeMail').val() == '') {
        $('#txtComposeMail').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Message is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    var data = $("#frmReply").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/Enquiries/SendMail/",

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




