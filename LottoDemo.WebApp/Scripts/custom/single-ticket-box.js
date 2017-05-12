
TicketBoxControl1 = null;
TicketBoxControl2 = null;
TicketBoxControl3 = null;

(function ($) {
    TicketBoxControl1 = new SingleTicketBox("#ticketBox_1", jQuery);
    TicketBoxControl2 = new SingleTicketBox("#ticketBox_2", jQuery);
    TicketBoxControl3 = new SingleTicketBox("#ticketBox_3", jQuery);
}(jQuery));

function SingleTicketBox(ticketBoxId, $) {

    var jqSingleTicket = null;
    var ticketBoxIndex = parseInt(ticketBoxId.split("_")[1]);

    var drawBallsList = [];
    var bonusBallsList = [];

    $(document).ready(function () {

        jqSingleTicket = $(ticketBoxId);

        // Bing quick pick button
        jqSingleTicket.find("#quickPickButton").click(function () {
            autoGenerateNumbers();
        });

        // Delete whole selection on click
        jqSingleTicket.find("#pick_remove > span").click(function () {
            clearSelection();
        });

        // Bing draw balls cells
        jqSingleTicket.find("td.drawBallCell").click(function () {
            updateBallSelection($(this), addDrawBall, removeDrawBall);
        });

        // Bing bonus balls cells
        jqSingleTicket.find("td.bonusBallCell").click(function () {
            updateBallSelection($(this), addBonusBall, removeBonusBall);
        });
    });

    var addDrawBall = function (ballValue) {
        drawBallsList.push(ballValue);
    };

    var addBonusBall = function (ballValue) {
        bonusBallsList.push(ballValue);
    };

    var removeDrawBall = function (ballValue) {
        drawBallsList.pop(ballValue);
    };

    var removeBonusBall = function (ballValue) {
        bonusBallsList.pop(ballValue);
    };

    var updateBallSelection = function (cell, addValueHandler, removeValueHandler) {

        // Stopp adding new numbers if the limit is reached
        if (!cell.hasClass("selected-active") && cell.hasClass("drawBallCell") && drawBallsList.length == GameTicketSettings.ballsCount) {
            return;
        }
        else if (!cell.hasClass("selected-active") && cell.hasClass("bonusBallCell") && bonusBallsList.length == GameTicketSettings.bonusBallsCount) {
            return;
        }

        var hiddenFieldList = null;
        var cellValue = parseInt(cell.text());

        cell.toggleClass("selected-active");
        if (cell.hasClass("selected-active")) {
            addValueHandler(cellValue);
        }
        else {
            removeValueHandler(cellValue);
        }

        // Set the hidden field lists with ticket numbers
        if (cell.hasClass("drawBallCell")) {
            setDrawBallsHiddenValue();
        }
        else if (cell.hasClass("bonusBallCell")) {
            setBonusBallsHiddenValue();
        }

        var bAddErrorBorder =
            bonusBallsList.length > 0 && bonusBallsList.length < GameTicketSettings.bonusBallsCount
            || bonusBallsList.length == GameTicketSettings.bonusBallsCount && drawBallsList.length == 0;

        bAddErrorBorder =
            bAddErrorBorder
            || drawBallsList.length > 0 && drawBallsList.length < GameTicketSettings.ballsCount
            || drawBallsList.length == GameTicketSettings.ballsCount && bonusBallsList.length == 0;

        // full incomplete
        if (drawBallsList.length == 0 && bonusBallsList.length == 0) {
            jqSingleTicket.removeClass("full");
            jqSingleTicket.removeClass("incomplete");
        }
        else if (bAddErrorBorder) {
            jqSingleTicket.removeClass("full");
            if (!jqSingleTicket.hasClass("incomplete")) {
                jqSingleTicket.addClass("incomplete");
            }
        }
        else if (drawBallsList.length == GameTicketSettings.ballsCount && bonusBallsList.length == GameTicketSettings.bonusBallsCount) {
            jqSingleTicket.addClass("full");
        }
    }

    var autoGenerateNumbers = function () {

        var settings = {};
        settings.drawBallsCount = GameTicketSettings.ballsCount;
        settings.bonusBallsCount = GameTicketSettings.bonusBallsCount;

        var requestUrl = AutoGenerateNumbersUrl + "?drawBallsCount=" + GameTicketSettings.ballsCount;
        var requestParams = {
            drawBallsCount: GameTicketSettings.ballsCount,
            bonusBallsCount: GameTicketSettings.bonusBallsCount,
            minBonusBallNumber: GameTicketSettings.minBonusBallNumber,
            maxBonusBallNumber: GameTicketSettings.maxBonusBallNumber,
            drawBallMaxNumber: GameTicketSettings.drawBallMaxNumber
        };

        $.ajax({
            url: AutoGenerateNumbersUrl,
            type: 'GET',
            dataType: 'json',
            data: requestParams,
            success: function (data, textStatus, xhr) {
                updateTicketBox(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    };

    var updateTicketBox = function (data) {

        if (data == null || data == undefined) {
            return;
        }

        clearSelection();

        drawBallsList = data.ballsList;
        bonusBallsList = data.bonusBallsList;
        setDrawBallsHiddenValue();
        setBonusBallsHiddenValue();

        var ballsCells = jqSingleTicket.find("td.drawBallCell");
        ballsCells.filter(function () {
            var cellValue = parseInt($.text([this]));
            return drawBallsList.indexOf(cellValue) > -1;
        }).addClass("selected-active");

        var bonusBallsCells = jqSingleTicket.find("td.bonusBallCell");
        bonusBallsCells.filter(function () {
            var cellValue = parseInt($.text([this]));
            return bonusBallsList.indexOf(cellValue) > -1;
        }).addClass("selected-active");

        jqSingleTicket.addClass("full");
    };

    var clearSelection = function () {
        drawBallsList = [];
        bonusBallsList = [];
        setDrawBallsHiddenValue();
        setBonusBallsHiddenValue();

        jqSingleTicket.find("td.drawBallCell").removeClass("selected-active");
        jqSingleTicket.find("td.bonusBallCell").removeClass("selected-active");
        jqSingleTicket.removeClass("full");
        jqSingleTicket.removeClass("incomplete");
    };

    var setDrawBallsHiddenValue = function () {
        $("#DrawBallsTicket" + ticketBoxIndex).val(JSON.stringify(drawBallsList));;
    }

    var setBonusBallsHiddenValue = function () {
        $("#BonusBallsTicket" + ticketBoxIndex).val(JSON.stringify(bonusBallsList));;
    }

    this.clearTicketBlank = function () {
        clearSelection();
    };

    this.updateLotteryTicketBox = function (data) {
        updateTicketBox(data);
    };

    this.isCompleted = function () {
        return drawBallsList.length == GameTicketSettings.ballsCount
            && bonusBallsList.length == GameTicketSettings.bonusBallsCount;
    };
};