$(() => {

    LoadAppointmentsNotifications();

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalrServer").build();
    connection.start();
    connection.on("LoadAppointmentsNotifications", function () {

        LoadAppointmentsNotifications();
    })

    LoadAppointmentsNotifications();

    function LoadAppointmentsNotifications() {
               
        $.get("/Admin/Appointments/GetSumAppointments/", function (data, status) {

            console.log(data);

            $("#txtCountNotifications").text(data.data);
  

        });
    };



})
