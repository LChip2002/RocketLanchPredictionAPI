using Microsoft.AspNetCore.Mvc;
using RLP_API.Models;
using RLP_API.Services;
using RLP_DB.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RLP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RLP_Controller : ControllerBase
    {
        private readonly LaunchQueryService _launchQueryService;
        private readonly PredictionQueryService _predictionQueryService;

        // Constructor
        public RLP_Controller()
        {
            // Initialization code here
            _launchQueryService = new LaunchQueryService();
            _predictionQueryService = new PredictionQueryService();
        }

        /// <summary>
        /// GetLaunches endpoint
        /// </summary>
        /// <param name="launchQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLaunches")]
        public async Task<IActionResult> GetLaunches([FromQuery] LaunchQuery launchQuery)
        {
            // Retrieve launch data from the database
            List<LaunchEntry> launchEntries = await _launchQueryService.GetLaunchDataAsync(launchQuery);

            // Checks if any entries are returned
            if (launchEntries.Count == 0)
            {
                return NoContent();
            }

            // Return the launch data
            return Ok(launchEntries);
        }

        /// <summary>
        /// GetPredictions endpoint
        /// </summary>
        /// <param name="predictionQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPredictions")]
        public async Task<IActionResult> GetGeneratedPredictions([FromQuery] PredictionQuery predictionQuery)
        {
            // Retrieve prediction data from the database
            List<LaunchPrediction> predictions = await _predictionQueryService.GetPredictionDataAsync(predictionQuery);

            // Checks if any entries are returned
            if (predictions.Count == 0)
            {
                return NoContent();
            }

            return Ok(predictions);
        }

        /// <summary>
        /// CreatePrediction endpoint
        /// </summary>
        /// <param name="predictionMaker"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePrediction")]
        public async Task<IActionResult> CreatePrediction([FromQuery] PredictionMaker predictionMaker)
        {
            // Returns the generated prediction data after querying the ML model
            PredictionMakeDto prediction = await _predictionQueryService.MakePredictionAsync(predictionMaker);

            // Checks if any entries are returned
            if (prediction.PredictedStatus == null)
            {
                return NoContent();
            }

            return Ok(prediction);
        }
    }
}