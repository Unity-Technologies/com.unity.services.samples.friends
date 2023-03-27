using System;
using System.Threading.Tasks;

namespace Unity.Samples.UI
{
    public static class PopUpEvents
    {
        /// <summary>
        /// Should only have one listener at a time. When awaited, it will only await the first event in the stack.
        /// </summary>
        public static event Func<string, string, string, Task<int>> PopupRequestedAsync;

        /// <summary>
        /// Awaits a the first Popup Task in the PopupRequestedAsync
        /// </summary>
        /// <param name="message"></param>
        /// <param name="option1Text"></param>
        /// <param name="option2Text"></param>
        /// <returns></returns>
        public static async Task<int> ShowPopup(string message, string option1Text, string option2Text)
        {
            var eventHandler = PopupRequestedAsync;
            if (eventHandler != null)
            {
                return await eventHandler.Invoke(message, option1Text, option2Text);
            }
            else
            {
                return -1; 
            }
        }
    }
}
