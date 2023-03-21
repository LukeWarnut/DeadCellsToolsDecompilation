using System;
using System.Windows.Forms;

namespace ModTools
{
	public class Error
	{
		public static void Show(Exception _e, bool _bShowMessage)
		{
			Error.Show(_bShowMessage, _e.Message, _e.StackTrace);
		}

		public static void Show(bool _bShowMsgBox, string _message, string _callstack)
		{
			if (_bShowMsgBox)
			{
				MessageBox.Show(_message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			Log.Error(_message, _callstack);
		}
	}
}
