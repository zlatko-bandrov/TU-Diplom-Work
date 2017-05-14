
UserCartManager = null;

(function ($) {

    UserCartManager = new UserCartControl($);

}(jQuery));

function UserCartControl($) {

    var timerInterval = 1000;

    $(document).ready(function () {

        $("#addTicketsToCart").click(function (evt) {
            if (!areThereCompletedTickets()) {
                evt.preventDefault();
                evt.stopPropagation();
                return false;
            }
        });

        // Tp update the ticket clock
        if (EnableDrawTimer == true) {
            updateDrawTimer();
        }
    });

    var updateDrawTimer = function () {
        $.ajax({
            url: UpdateTicketTimerUrl,
            type: 'POST',
            dataType: 'json',
            data: {
                nextDrawTimeTicks: GameNextDrawTime,
                redirectUrl: CurrentPageUrl
            },
            success: function (data, textStatus, xhr) {
                updateTimerHtml(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });

        if (EnableDrawTimer == true) {
            setTimeout(updateDrawTimer, timerInterval);
        }
    };

    var updateTimerHtml = function (jsonData) {
        $("#ticketDrawTimer span#days:first").text(jsonData.days);
        $("#ticketDrawTimer span#hours:first").text(jsonData.hours);
        $("#ticketDrawTimer span#minutes:first").text(jsonData.minutes);
        $("#ticketDrawTimer span#seconds:first").text(jsonData.seconds);
    };

    var areThereCompletedTickets = function () {
        return TicketBoxControl1.isCompleted()
            || TicketBoxControl2.isCompleted()
            || TicketBoxControl3.isCompleted();
    };
};