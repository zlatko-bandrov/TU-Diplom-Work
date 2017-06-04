using LottoDemo.BusinessLogic.Services;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.WebAPI;
using System;
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

            gameNumbers.BallsList = LottoDrawService.GenerateDrawNumbers(1, gameSettings.DrawBallMaxNumber, gameSettings.DrawBallsCount);
            gameNumbers.BonusBallsList =
                LottoDrawService.GenerateDrawNumbers(
                    gameSettings.MinBonusBallNumber,
                    gameSettings.MaxBonusBallNumber,
                    gameSettings.BonusBallsCount);

            return Json(gameNumbers);
        }

        [HttpPost]
        public JsonResult<DrawingTimerModel> GetNextDrawTimeLeft([FromBody]NextDrawTimeLeftModel settings)
        {
            DateTime nextDrawTime = new DateTime(long.Parse(settings.NextDrawTimeTicks));
            TimeSpan timeLeft = nextDrawTime - DateTime.Now;

            var model = new DrawingTimerModel
            {
                Days = timeLeft.Days,
                Hours = timeLeft.Hours,
                Minutes = timeLeft.Minutes,
                Seconds = timeLeft.Seconds
            };

            if (timeLeft.TotalSeconds <= 5)
            {
                // Redirect to current page to disable the timer
                RedirectToRoute(settings.RedirectUrl, null);
            }

            return Json(model);
        }

        [HttpGet]
        public JsonResult<LottoGameSettings> GetGameSettings(string gameKey)
        {
            Guid gameKeyId = Guid.Parse(gameKey);
            var publishedContent = Umbraco.TypedContent(gameKeyId);
            var model = new LottoGameSettings(publishedContent);

            return Json(model);
        }

        [HttpPut]
        public IHttpActionResult UpdateGameJackpot(Guid gameKey, decimal newJackpot)
        {
            var gameContent = Services.ContentService.GetById(gameKey);
            if (gameContent != null)
            {
                gameContent.SetValue("Jackpot", newJackpot);
                Services.ContentService.SaveAndPublishWithStatus(gameContent);
            }
            return Ok();
        }
    }
}