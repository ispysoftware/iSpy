/*-------------------------------------------------------------------------------
 *	Author: Tamir Khason, 2004
 *	Email: tamir@khason.biz
 *	Web: http://www.dotnet.us/
 * 
 *	Freeware: Please do not remove this header
 *
 *	File: P.cs
 *
 *	Description:	PELCO P Protocol Implementation dot.NET Way
 * This is GPL software. Do with it as you want, but feed us back any improvements. 
 * 
 * This is a simple class to control a PELCO PTZ camera via RS422/485
 * 'P' protocol. It supports all of the commands including UP, DOWN, IN, OUT, LEFT,
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
	/// dot.NET Implementation of Pelco P Protocol
	/// </summary>
	public class P
	{
		private const byte STX = 0xA0;
		private const byte ETX = 0xAF;
		
		#region Pan and Tilt Commands
		#region Data1
		private const byte FocusFar =	0x01;
		private const byte FocusNear =	0x02;
		private const byte IrisOpen =	0x04;
		private const byte IrisClose =	0x08;
		private const byte CameraOnOff = 0x10;
		private const byte AutoscanOn =	0x20;
		private const byte CameraOn =	0x40;
		#endregion

		#region Data2
		private const byte PanRight =	0x02;
		private const byte PanLeft =	0x04;
		private const byte TiltUp =		0x08;
		private const byte TiltDown =	0x10;
		private const byte ZoomTele =	0x20;
		private const byte ZoomWide =	0x40;
		#endregion

		#region Data3
		private const byte PanSpeedMin = 0x00;
		private const byte PanSpeedMax = 0x40;
		#endregion

		#region Data4
		private const byte TiltSpeedMin = 0x00;
		private const byte TiltSpeedMax = 0x3F;
		#endregion
		#endregion

		#region Enums
		public enum PresetAction {Set,Clear,Goto}
		public enum PatternAction {Start,Stop,Run}
		public enum Action {Start,Stop}
		public enum LensSpeed {Low=0x00,Medium=0x01,High=0x02,Turbo=0x03}
		public enum Pan {Left = PanLeft,Right = PanRight}
		public enum Tilt {Up = TiltUp,Down = TiltDown}
		public enum Iris {Open = IrisOpen,Close = IrisClose}
		public enum Zoom {Wide = ZoomWide,Tele = ZoomTele}
		public enum Switch {On,Off}
		public enum Focus {Near = FocusNear,Far = FocusFar}
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

		public byte[] AutoScan(uint deviceAddress, Action action)
		{
			byte mAction;
			if(action == Action.Start)
				mAction = 0x09;
			else
				mAction = 0x0B;
			return Message.GetMessage(deviceAddress,0x00,mAction,0x00,0x00);
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
			var mBytes = new byte[encoding.GetByteCount(text)*8];
			int i = 0;

		    foreach(char ch in text)
			{
				byte mScrPosition = Convert.ToByte(i/8);
				byte mAsciIchr = Convert.ToByte(ch);
                System.Array.Copy(Message.GetMessage(deviceAddress, 0x00, 0x15, mScrPosition, mAsciIchr), 0, mBytes, i, 8);
				i = i + 8;
			}

			return mBytes;
		}

		public byte[] ClearScreen(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x17,0x00,0x00);
		}

		public byte[] AlarmAcknowledge(uint deviceAddress, uint alarmID)
		{
			if(alarmID < 1 & alarmID>8)
				throw new Exception("Only 8 alarms allowed for Pelco P implementation");
			return Message.GetMessage(deviceAddress,0x00,0x19,0x00,Convert.ToByte(alarmID));
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

		#endregion

		#region Base Command Set

		public byte[] CameraSwitch(uint deviceAddress,Switch action)
		{
			byte m_action = CameraOnOff;
			if(action == Switch.On)
				m_action += CameraOnOff; //Maybe wrong !!!
			return Message.GetMessage(deviceAddress,m_action,0x00,0x00,0x00);
			
		}

		public byte[] CameraIrisSwitch(uint deviceAddress,Iris action)
		{
			return Message.GetMessage(deviceAddress,(byte)action,0x00,0x00,0x00);
		}

		public byte[] CameraFocus(uint deviceAddress,Focus action)
		{
			return Message.GetMessage(deviceAddress,(byte)action,0x00,0x00,0x00);
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
			byte[] m_bytes = new byte[8];
			byte[] m_tiltMessage = CameraTilt(deviceAddress,tiltAction,tiltSpeed);
			byte[] m_panMessage = CameraPan(deviceAddress,panAction,panSpeed);
			//m_tiltMessage.CopyTo(m_bytes,0);
			//m_panMessage.CopyTo(m_bytes,9);
			/*m_bytes[0] = m_tiltMessage[0];
			m_bytes[1] = m_tiltMessage[1];
			m_bytes[2] = m_tiltMessage[2];
			m_bytes[3] = (byte)(m_tiltMessage[3]+m_panMessage[3]);
			m_bytes[4] = (byte)(+m_panMessage[4]);
			m_bytes[5] = (byte)(+m_panMessage[5]);
			m_bytes[6] = m_tiltMessage[6];
			m_bytes[7] = m_tiltMessage[7];*/
			m_bytes = Message.GetMessage(deviceAddress,0x00,(byte)(m_tiltMessage[3]+m_panMessage[3]),
				m_panMessage[4],m_tiltMessage[5]);
			return m_bytes;

		}

		public byte[] CameraStop(uint deviceAddress)
		{
			return Message.GetMessage(deviceAddress,0x00,0x00,0x00,0x00);
		}




		#endregion

		public struct Message
		{
			public static byte Address;
			public static byte CheckSum;
			public static byte Data1,Data2,Data3,Data4;

			public static byte[] GetMessage(uint address, byte data1, byte data2, byte data3, byte data4)
			{
				if (address<0 & address>32)
					throw new Exception("Protocol Pelco P support 32 devices only");

				Address = Byte.Parse((address-1).ToString(CultureInfo.InvariantCulture));
				Data1 = data1;
				Data2 = data2;
				Data3 = data3;
				Data4 = data4;

				CheckSum = (byte)(STX ^ Address ^ Data1 ^ Data2 ^ Data3 ^ Data4 ^ ETX);

				return new []{STX,Address,Data1,Data2,Data3,Data4,ETX,CheckSum};
			}
			
		}
	}

	 
}
