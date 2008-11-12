using System.Windows.Forms;

namespace Waddu.Forms
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
