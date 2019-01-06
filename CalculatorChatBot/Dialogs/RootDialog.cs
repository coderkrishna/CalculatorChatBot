﻿namespace CalculatorChatBot.Dialogs
{
    using CalculatorChatBot.BotHelpers;
    using Microsoft.Bot.Builder.Dialogs;
    using CalculatorChatBot.Dialogs.Arithmetic;
    using CalculatorChatBot.Dialogs.Statistics;
    using Microsoft.Bot.Builder.Scorables;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Teams.Models;
    using System;
    using System.Threading.Tasks;
    using CalculatorChatBot.Dialogs.Geometry;

    [Serializable]
    public class RootDialog : DispatchDialog
    {
        #region Hello World like functionality
        [RegexPattern("hello")]
        [RegexPattern("hi")]
        [ScorableGroup(1)]
        public async Task RunHelloDialog(IDialogContext context, IActivity activity)
        {
            var helloResult = activity as Activity;
            context.Call(new HelloDialog(helloResult), EndDialog);
        }

        [RegexPattern("greet everyone")]
        [ScorableGroup(1)]
        public async Task RunGreetDialog(IDialogContext context, IActivity activity)
        {
            var channelData = context.Activity.GetChannelData<TeamsChannelData>();
            if (channelData.Team != null)
            {
                context.Call(new GreetDialog(), EndDialog);
            }
            else
            {
                await context.PostAsync("I'm sorry, you can only do this from within a Team.");
                context.Done<object>(null);
            }
        }
        #endregion

        #region Arithmetic Operations
        [RegexPattern("add")]
        [RegexPattern("addition")]
        [RegexPattern("sum")]
        [ScorableGroup(1)]
        public async Task RunAddDialog(IDialogContext context, IActivity activity)
        {
            var result = activity as Activity;
            context.Call(new AddDialog(result), EndDialog);
        }

        [RegexPattern("subtract")]
        [RegexPattern("subtraction")]
        [RegexPattern("difference")]
        [ScorableGroup(1)]
        public async Task RunSubtractDialog(IDialogContext context, IActivity activity)
        {
            var result = activity as Activity;
            context.Call(new SubtractDialog(result), EndDialog);
        }

        [RegexPattern("product")]
        [RegexPattern("multiplication")]
        [RegexPattern("multiply")]
        [ScorableGroup(1)]
        public async Task RunMultiplyDialog(IDialogContext context, IActivity activity)
        {
            var multiResult = activity as Activity;
            context.Call(new MultiplyDialog(multiResult), EndDialog);
        }

        [RegexPattern("divide")]
        [RegexPattern("division")]
        [RegexPattern("quotient")]
        [ScorableGroup(1)]
        public async Task RunDivideDialog(IDialogContext context, IActivity activity)
        {
            var divideResult = activity as Activity;
            context.Call(new DivideDialog(divideResult), EndDialog);
        }

        [RegexPattern("remainder")]
        [RegexPattern("modulo")]
        [ScorableGroup(1)]
        public async Task RunModuloDialog(IDialogContext context, IActivity activity)
        {
            var modResult = activity as Activity;
            context.Call(new ModuloDialog(modResult), EndDialog);
        }
        #endregion

        #region Statistical Operations
        /// <summary>
        /// This function calls the dialog to calculate the mean/average
        /// </summary>
        [RegexPattern("average")]
        [RegexPattern("mean")]
        [ScorableGroup(1)]
        public async Task RunAverageDialog(IDialogContext context, IActivity activity)
        {
            var meanResult = activity as Activity;
            context.Call(new AverageDialog(meanResult), EndDialog);
        }

        /// <summary>
        /// This will call the dialog to calculate the median
        /// </summary>
        [RegexPattern("median")]
        [ScorableGroup(1)]
        public async Task RunMedianDialog(IDialogContext context, IActivity activity)
        {
            var medianResult = activity as Activity;
            context.Call(new MedianDialog(medianResult), EndDialog);
        }

        /// <summary>
        /// This function will call the dialog to calculate the mode
        /// </summary>
        [RegexPattern("mode")]
        [ScorableGroup(1)]
        public async Task RunModeDialog(IDialogContext context, IActivity activity)
        {
            var modeResult = activity as Activity;
            context.Call(new ModeDialog(modeResult), EndDialog); 
        }

        [RegexPattern("range")]
        [ScorableGroup(1)]
        public async Task RunRangeDialog(IDialogContext context, IActivity activity)
        {
            var rangeResult = activity as Activity;
            context.Call(new RangeDialog(rangeResult), EndDialog);
        }
        #endregion

        #region Geometric Operations
        [RegexPattern("pythagoras")]
        [RegexPattern("pythagorean")]
        [ScorableGroup(1)]
        public async Task RunPythagoreanDialog(IDialogContext context, IActivity activity)
        {
            var pythagResult = activity as Activity;
            context.Call(new PythagoreanDialog(pythagResult), EndDialog);
        }
        #endregion

        #region Generic help
        [RegexPattern("help")]
        [ScorableGroup(1)]
        public async Task GetHelp(IDialogContext context, IActivity activity)
        {
            // Send the generic help message
            await context.PostAsync(MessageHelpers.CreateHelpMessage(""));
            context.Done<object>(null);
        } 
        #endregion

        [MethodBind]
        [ScorableGroup(2)]
        public async Task Default(IDialogContext context, IActivity activity)
        {
            // Send message
            await context.PostAsync("I'm sorry, but I didn't understand.");
            context.Done<object>(null); 
        }

        /// <summary>
        /// This method would always ensure that the RootDialog would be popped off
        /// the DialogStack whenever the bot is shutting down
        /// </summary>
        public async Task EndDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
    }
}