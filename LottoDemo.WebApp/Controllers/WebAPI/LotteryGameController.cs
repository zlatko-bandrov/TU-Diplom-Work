using LottoDemo.BusinessLogic.Services;
using LottoDemo.Entities.Models.WebAPI;
using System.Web.Http;
using System.Web.Http.Results;
using Umbraco.Web.WebApi;

namespace LottoDemo.WebApp.Controllers.WebAPI
{
    public class LotteryGameController : UmbracoApiController
    {
        [HttpGet]
        public JsonResult<AutoGenerateNumbersResult> GetRandomNumbers([FromUri]GameDrawSettings gameSettings)
        {
            var gameNumbers = new AutoGenerateNumbersResult();
            var drawingService = new GameDrawingService();

            gameNumbers.BallsList = drawingService.GenerateDrawNumbers(1, gameSettings.DrawBallMaxNumber, gameSettings.DrawBallsCount);
            gameNumbers.BonusBallsList =
                drawingService.GenerateDrawNumbers(
                    gameSettings.MinBonusBallNumber,
                    gameSettings.MaxBonusBallNumber,
                    gameSettings.BonusBallsCount);

            return Json(gameNumbers);
        }
    }
}