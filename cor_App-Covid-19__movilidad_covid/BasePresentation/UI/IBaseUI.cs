using System;

namespace Presentation.UI.Base
{
    public interface IBaseUI
    {
        void ShowLoading();
        void HideLoading();
        void HideKeyboard();
        void ShowDialog(String text, String buttonText, Action action);
        void ShowDialog(String text, String NegativeButton, String PositiveButton, Action positiveAction);
        void ShowDialog(String text, String NegativeButton, Action negativeAction, String PositiveButton, Action positiveAction);
    }
}
