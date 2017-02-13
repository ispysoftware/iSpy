/*-------------------------------------------------------------------------------
 *	Author: Tamir Khason, 2004
 *	Email: tamir@khason.biz
 *	Web: http://www.dotnet.us/
 * 
 *	Freeware: Please do not remove this header
 *
 *	File: D.cs
 *
 *	Description:	PELCO D Protocol Implementation dot.NET Way
 * This is GPL software. Do with it as you want, but feed us back any improvements. 
 * 
 * This is a simple class to control a PELCO PTZ cameras, matrix switching systems and
 * receiver/drivers via RS422/485 'D' protocol. 
 * It supports all of the commands including UP, DOWN, IN, OUT, LEFT,
 * RIGHT, NEAR, FAR, as well as all other extended commands.  
 * 
 * To use this, you need to put a RS232->RS422 adapter on the output
 * of your desired serial port. 
 * 
 * The Pelco doesn't return ANY usefull info back, so you only really need 2-wire support 
 * (one way) communications out.
 * 
 * 
 * Version: 1.4
 * ------------------------------------------------------------------------------*/
using System;
using System.Globalization;

namespace iSpyApplication.Pelco
{
	/// <summary>
	/// dot.NET Implementation of Pelco D Protocol
	/// </summary>
	public class D
	{
		private const byte STX = 0xFF;

		#region Pan and Tilt Commands
		#region Command1
		private const byte FocusNear =	0x01;
		private const byte IrisOpen =	0x02;
		private const byte IrisClose =	0x04;
		private const byte CameraOnOff = 0x08;
		private const byte AutoManualScan =	0x10;
		private const byte Sense =	0x80;
		#endregion

		#region Command2
		private const byte PanRight =	0x02;
		private const byte PanLeft =		0x04;
		private const byte TiltUp =		0x08;
		private const byte TiltDown =	0x10;
		private const byte ZoomTele =	0x20;
		private const byte ZoomWide =	0x40;
		private const byte FocusFar =	0x80;
		#endregion

		#region Data1
		private const byte PanSpeedMin = 0x00;
		private const byte PanSpeedMax = 0xFF;
		#endregion

		#region Data2
		private const byte TiltSpeedMin = 0x00;
		private const byte TiltSpeedMax = 0x3F;
		#endregion
		#endregion

		#region Enums
		public enum PresetAction {Set,Clear,Goto}
		public enum AuxAction {Set=0x09,Clear=0x0B}
		public enum Action {Start,Stop}
		public enum LensSpeed {Low=0x00,Medium=0x01,High=0x02,Turbo=0x03}
		public enum PatternAction {Start,Stop,Run}
		public enum SwitchAction {Auto=0x00,On=0x01,Off=0x02}
		public enum Switch {On=0x01,Off=0x02}
		public enum Focus {Near = FocusNear,Far = FocusFar}
		public enum Zoom {Wide = ZoomWide,Tele = ZoomTele}
		public enum Tilt {Up = TiltUp,Down = TiltDown}
		public enum Pan {Left = PanLeft,Right = PanRight}
		public enum Scan {Auto, Manual}
		public enum Iris {Open = IrisOpen,Close = IrisClose}
		#endregion

		#region Extended Command Set
		public byte[] Preset(uint deviceAddress, byte preset, PresetAction action)
		{
			byte mAction;
			switch (action)
			{
				case PresetAction.Set:
					mAction = 0x03;
					break;
				case PresetAction.Clear:
					mAction = 0x05;
					break;
				case PresetAction.Goto:
					mAction = 0x07;
					break;
				default:
					mAction = 0x03;
					break;
			}
			return Message.GetMessage(deviceAddress,0x00,mAction,0x00,preset);
		}

		public byte[] Flip(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x07,0x00,0x21);
		}

		public byte[] ZeroPanPosition(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x07,0x00,0x22);
		}

		public byte[] SetAuxiliary(uint deviceAddress,byte auxiliaryId, AuxAction action)
		{
			if(auxiliaryId<0x00)
				auxiliaryId = 0x00;
			else if(auxiliaryId>0x08)
				auxiliaryId = 0x08;
			return Message.GetMessage(deviceAddress,0x00,(byte)action,0x00,auxiliaryId);
		}

		public byte[] RemoteReset(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x0F,0x00,0x00);
		}
		public byte[] Zone(uint deviceAddress,byte zone, Action action)
		{
			if(zone<0x01 & zone>0x08)
				throw new Exception("Zone value should be between 0x01 and 0x08 include");
			byte mAction;
			if(action == Action.Start)
				mAction = 0x11;
			else
				mAction = 0x13;

			return Message.GetMessage(deviceAddress,0x00,mAction,0x00,zone);
		}

		public byte[] WriteToScreen(uint deviceAddress,string text)
		{
			if(text.Length > 40)
				text = text.Remove(40,text.Length-40);
			System.Text.Encoding encoding = System.Text.Encoding.ASCII;
			byte[] mBytes = new byte[encoding.GetByteCount(text)*7];
			int i = 0;

		    foreach(char ch in text)
			{
				byte mScrPosition = Convert.ToByte(i/7);
				byte mAsciIchr = Convert.ToByte(ch);
                System.Array.Copy(Message.GetMessage(deviceAddress, 0x00, 0x15, mScrPosition, mAsciIchr), 0, mBytes, i, 7);
				i = i + 7;
			}

			return mBytes;
		}

		public byte[] ClearScreen(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x17,0x00,0x00);
		}

		public byte[] AlarmAcknowledge(uint deviceAddress, uint alarmId)
		{
			if(alarmId < 1 & alarmId>8)
				throw new Exception("Only 8 alarms allowed for Pelco P implementation");
			return Message.GetMessage(deviceAddress,0x00,0x19,0x00,Convert.ToByte(alarmId));
		}

		public byte[] ZoneScan(uint deviceAddress,Action action)
		{
			byte mAction;
			if(action == Action.Start)
				mAction = 0x1B;
			else
				mAction = 0x1D;
			return Message.GetMessage(deviceAddress,0x00,mAction,0x00,0x00);
		}

		public byte[] Pattern(uint deviceAddress,PatternAction action)
		{
			byte mAction;
			switch (action)
			{
				case PatternAction.Start:
					mAction = 0x1F;
					break;
				case PatternAction.Stop:
					mAction = 0x21;
					break;
				case PatternAction.Run:
					mAction = 0x23;
					break;
				default:
					mAction = 0x23;
					break;
			}
			return Message.GetMessage(deviceAddress,0x00,mAction,0x00,0x00);
		}

		public byte[] SetZoomLensSpeed(uint deviceAddress, LensSpeed speed)
		{
			return Message.GetMessage(deviceAddress,0x00,0x25,0x00,(byte)speed);
		}

		public byte[] SetFocusLensSpeed(uint deviceAddress, LensSpeed speed)
		{
			return Message.GetMessage(deviceAddress,0x00,0x27,0x00,(byte)speed);
		}

		public byte[] ResetCamera(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x29,0x00,0x00);
		}
		public byte[] AutoFocus(uint deviceAddress, SwitchAction action)
		{
			return Message.GetMessage(deviceAddress,0x00,0x2B,0x00,(byte)action);
		}
		public byte[] AutoIris(uint deviceAddress, SwitchAction action)
		{
			return Message.GetMessage(deviceAddress,0x00,0x2D,0x00,(byte)action);
		}
		public byte[] AGC(uint deviceAddress, SwitchAction action)
		{
			return Message.GetMessage(deviceAddress,0x00,0x2F,0x00,(byte)action);
		}
		public byte[] BackLightCompensation(uint deviceAddress, Switch action)
		{
			return Message.GetMessage(deviceAddress,0x00,0x31,0x00,(byte)action);
		}
		public byte[] AutoWhiteBalance(uint deviceAddress, Switch action)
		{
			return Message.GetMessage(deviceAddress,0x00,0x33,0x00,(byte)action);
		}

		public byte[] EnableDevicePhaseDelayMode(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x35,0x00,0x00);
		}
		public byte[] SetShutterSpeed(uint deviceAddress,byte speed)
		{
			return Message.GetMessage(deviceAddress,0x00,0x37,speed,speed);//Not sure about
		}
		public byte[] AdjustLineLockPhaseDelay(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x39,0x00,0x00);
		}
		public byte[] AdjustWhiteBalanceRB(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x3B,0x00,0x00);
		}
		public byte[] AdjustWhiteBalanceMG(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x3D,0x00,0x00);
		}
		public byte[] AdjustGain(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x3F,0x00,0x00);
		}
		public byte[] AdjustAutoIrisLevel(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x41,0x00,0x00);
		}
		public byte[] AdjustAutoIrisPeakValue(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x43,0x00,0x00);
		}
		public byte[] Query(uint deviceAddress)
		{
			throw new Exception("Did not implemented");
			//return Message.GetMessage(deviceAddress,0x00,0x45,0x00,0x00);
		}
		#endregion

		#region Base Command Set

		public byte[] CameraSwitch(uint deviceAddress,Switch action)
		{
			byte mAction = CameraOnOff;
			if(action == Switch.On)
				mAction = CameraOnOff + Sense;
			return Message.GetMessage(deviceAddress,mAction,0x00,0x00,0x00);
			
		}

		public byte[] CameraIrisSwitch(uint deviceAddress,Iris action)
		{
			return Message.GetMessage(deviceAddress,(byte)action,0x00,0x00,0x00);
		}

		public byte[] CameraFocus(uint deviceAddress,Focus action)
		{
			if(action == Focus.Near)
				return Message.GetMessage(deviceAddress,(byte)action,0x00,0x00,0x00);
			return Message.GetMessage(deviceAddress,0x00,(byte)action,0x00,0x00);
		}

		public byte[] CameraZoom(uint deviceAddress,Zoom action)
		{
			return Message.GetMessage(deviceAddress,0x00,(byte)action,0x00,0x00);
		}

		public byte[] CameraTilt(uint deviceAddress,Tilt action, uint speed)
		{
			if(speed<TiltSpeedMin)
				speed = TiltSpeedMin;
			if(speed<TiltSpeedMax)
				speed = TiltSpeedMax;

			return Message.GetMessage(deviceAddress,0x00,(byte)action,0x00,(byte)speed);
		}

		public byte[] CameraPan(uint deviceAddress,Pan action, uint speed)
		{
			if(speed<PanSpeedMin)
				speed = PanSpeedMin;
			if(speed<PanSpeedMax)
				speed = PanSpeedMax;

			return Message.GetMessage(deviceAddress,0x00,(byte)action,(byte)speed,0x00);
		}

		public byte[] CameraPanTilt(uint deviceAddress,Pan panAction, uint panSpeed, Tilt tiltAction, uint tiltSpeed)
		{
		    byte[] mTiltMessage = CameraTilt(deviceAddress,tiltAction,tiltSpeed);
			byte[] mPanMessage = CameraPan(deviceAddress,panAction,panSpeed);
			byte[] mBytes = Message.GetMessage(deviceAddress,0x00,(byte)(mTiltMessage[3]+mPanMessage[3]),
			                                   mPanMessage[4],mTiltMessage[5]);
			return mBytes;

		}

		public byte[] CameraStop(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x00,0x00,0x00);
		}

		public byte[] CameraScan(uint deviceAddress,Scan scan)
		{
			byte mByte = AutoManualScan;
			if(scan == Scan.Auto)
				mByte = AutoManualScan+Sense;

			return Message.GetMessage(deviceAddress,mByte,0x00,0x00,0x00);

		}
        
		#endregion



		public struct Message
		{
			public static byte Address;
			public static byte CheckSum;
			public static byte Command1,Command2,Data1,Data2;

			public static byte[] GetMessage(uint address, byte command1, byte command2, byte data1, byte data2)
			{
				if (address<1 & address>256)
					throw new Exception("Protocol Pelco D support 256 devices only");
				
				Address = Byte.Parse((address).ToString(CultureInfo.InvariantCulture));
				Data1 = data1;
				Data2 = data2;
				Command1 = command1;
				Command2 = command2;

                CheckSum = (byte)((Address + Command1 + Command2 + Data1 + Data2) % 256);


				return new[]{STX,Address,Command1,Command2,Data1,Data2,CheckSum};
			}
			
		}
	}
}
