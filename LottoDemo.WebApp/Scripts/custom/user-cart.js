
UserCartManager = null;

(function ($) {

    UserCartManager = new UserCartControl($);

}(jQuery));

function UserCartControl($) {

    $(document).ready(function () {

        $("#addTicketsToCart").click(function (evt) {
            if (!areThereCompletedTickets()) {
                evt.preventDefault();
                evt.stopPropagation();
                return false;
            }
        });
    });

    var areThereCompletedTickets = function () {
        return TicketBoxControl1.isCompleted()
            || TicketBoxControl2.isCompleted()
            || TicketBoxControl3.isCompleted();
    };
};