using System.Windows.Forms;

namespace Waddu.UI.Forms
{
    public partial class ChangeLogForm : Form
    {
        public ChangeLogForm(string changeLog)
        {
            InitializeComponent();

            wbChangeLog.DocumentText = changeLog;
        }
    }
}
