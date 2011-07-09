﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Cosmos.VS.Debug;
using System.Threading;
using Cosmos.Compiler.Debug;

namespace Cosmos.Cosmos_VS_Windows
{
    public partial class RegistersUC : UserControl
    {
        public RegistersUC()
        {
            InitializeComponent();
            lablEAX.Content = "";
            lablAX.Content = "";
            lablAH.Content = "";
            lablAL.Content = "";
            lablEBX.Content = "";
            lablBX.Content = "";
            lablBH.Content = "";
            lablBL.Content = "";
            lablECX.Content = "";
            lablCX.Content = "";
            lablCH.Content = "";
            lablCL.Content = "";
            lablEDX.Content = "";
            lablDX.Content = "";
            lablDH.Content = "";
            lablDL.Content = "";
            lablEBP.Content = "";
            lablESI.Content = "";
            lablEDI.Content = "";
            lablESP.Content = "";
            lablEIP.Content = "";
            lablFlags.Content = "";
      }

        protected void UpdateRegisters(byte[] aData, int aOffset, Label a32, Label a16, Label a8Hi, Label a8Lo)
        {
            byte x8Lo = aData[aOffset];
            byte x8Hi = aData[aOffset + 1];
            SetLabel(a8Lo, x8Lo.ToString("X2"));
            SetLabel(a8Hi, x8Hi.ToString("X2"));
            SetLabel(a16, x8Hi.ToString("X2") + x8Lo.ToString("X2"));
            UpdateRegister32(aData, aOffset, a32);
        }

        protected void UpdateRegister32(byte[] aData, int aOffset, Label a32)
        {
            UInt32 x32 = (UInt32)
              (aData[aOffset + 3] << 24 |
              aData[aOffset + 2] << 16 |
              aData[aOffset + 1] << 8 |
              aData[aOffset]);
            SetLabel(a32, x32.ToString("X8"));
        }

        protected void SetLabel(Label aLabel, string aContent)
        {
            string xOldContent = (string)aLabel.Content;
            aLabel.Content = aContent;
            aLabel.Foreground = (xOldContent == aContent) ? Brushes.Black : Brushes.Red;
        }

        public void Update(byte[] aData)
        {
            //Push All
            //  Temp = (ESP);
            //  Push(EAX); 28
            //  Push(ECX); 24
            //  Push(EDX); 20
            //  Push(EBX); 16
            //  Push(Temp); 12 // Have to get from another source, ESP is already in DS when we pushall
            //  Push(EBP); 8
            //  Push(ESI); 4
            //  Push(EDI); 0
            // We get them from bottom up, so we receive them in reverse order as shown above. That is 0-3 is EDI.
            //
            // Additional ones sent manually (at end, not from stack)
            // ESP 32
            // EIP 36
            //
            UpdateRegisters(aData, 28, lablEAX, lablAX, lablAH, lablAL);
            UpdateRegisters(aData, 16, lablEBX, lablBX, lablBH, lablBL);
            UpdateRegisters(aData, 24, lablECX, lablCX, lablCH, lablCL);
            UpdateRegisters(aData, 20, lablEDX, lablDX, lablDH, lablDL);
            UpdateRegister32(aData, 8, lablEBP);
            UpdateRegister32(aData, 4, lablESI);
            UpdateRegister32(aData, 0, lablEDI);
            UpdateRegister32(aData, 32, lablESP);
            UpdateRegister32(aData, 36, lablEIP);
          //TODO: Flags
        }
    }
}