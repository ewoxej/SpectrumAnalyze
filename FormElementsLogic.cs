using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumAnalyzer
{
    enum APP_STATE
    {
        NoFile,
        OpenedSaved,
        OpenedUnsaved,
        Recording,
    }
    class FormElementsLogic
    {
        private Button btn_rec, btn_stop, btn_open, btn_save, btn_close,btn_play;
        private APP_STATE state;
        public FormElementsLogic(Button record,Button stop,Button open,Button save,Button close,Button play)
        {
            btn_rec = record;
            btn_stop = stop;
            btn_open = open;
            btn_save = save;
            btn_close = close;
            btn_play = play;
            changeState(APP_STATE.NoFile);
        }
        public void changeState(APP_STATE newState)
        {
            state = newState;
            switch(state)
            {
                case APP_STATE.NoFile:
                    {
                        btn_rec.Enabled = true;
                        btn_stop.Enabled = false;
                        btn_open.Enabled = true;
                        btn_save.Enabled = false;
                        btn_close.Enabled = false;
                        btn_play.Enabled = false;
                    }
                    break;
                case APP_STATE.OpenedSaved:
                    {
                        btn_rec.Enabled = true;
                        btn_stop.Enabled = false;
                        btn_open.Enabled = true;
                        btn_save.Enabled = false;
                        btn_close.Enabled = true;
                        btn_play.Enabled = true;
                    }
                    break;
                case APP_STATE.OpenedUnsaved:
                    {
                        btn_rec.Enabled = true;
                        btn_stop.Enabled = false;
                        btn_open.Enabled = true;
                        btn_save.Enabled = true;
                        btn_close.Enabled = true;
                        btn_play.Enabled = true;
                    }
                    break;
                case APP_STATE.Recording:
                    {
                        btn_rec.Enabled = false;
                        btn_stop.Enabled = true;
                        btn_save.Enabled = false;
                        btn_open.Enabled = false;
                        btn_play.Enabled = false;
                    }
                    break;
            }
        }
        public APP_STATE GetState()
        {
            return state;
        }
    }
}
