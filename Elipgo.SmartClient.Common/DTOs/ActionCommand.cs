using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
	public class ActionCommand
	{
		private PtzCommand _command;
		private double _parameter;
		private double _parameter2;
		private bool _isInvoked;
		private ButtonOrAxis _buttonOrAxis;

		public ActionCommand()
		{
			this.isInvoked = false;
			this.buttonOrAxis = ButtonOrAxis.None;
		}

		public ActionCommand(PtzCommand command, double parameter, bool invoked)
		{
			this.buttonOrAxis = ButtonOrAxis.None;
			this.isInvoked = invoked;
			this.command = command;
			this.Parameter = parameter;
		}

		public ActionCommand(PtzCommand command, ButtonOrAxis buttonOrAxis, double parameter, double parameter2, bool invoked)
		{
			this.buttonOrAxis = buttonOrAxis;
			this.isInvoked = invoked;
			this.command = command;
			this.Parameter = parameter;
			this.Parameter2 = parameter2;
		}

		public ActionCommand( double axisX, double axisY)
		{
			if(axisX != 0d && axisY != 0d)
			{
				if (axisX > 0 && axisY > 0)
					this.buttonOrAxis = ButtonOrAxis.RIGHTTOP;
				else if (axisX < 0 && axisY < 0)
					this.buttonOrAxis = ButtonOrAxis.LEFTDOWN;
				else if (axisX > 0 && axisY < 0)
					this.buttonOrAxis = ButtonOrAxis.RIGHTDOWN;
				else if (axisX < 0 && axisY > 0)
					this.buttonOrAxis = ButtonOrAxis.LEFTTOP;
				else
					this.buttonOrAxis = ButtonOrAxis.None;
				this.Parameter = axisX;
				this.Parameter2 = axisY;
			}
			else if( axisX != 0 )
			{
				this.buttonOrAxis = axisX >= 0 ? ButtonOrAxis.RIGHT_CONTROL : ButtonOrAxis.LEFT_CONTROL;
				this.Parameter2 = Math.Abs(axisX);
			}
			else
			{
				this.buttonOrAxis = axisY >= 0 ? ButtonOrAxis.UP_CONTROL : ButtonOrAxis.DOWN_CONTROL;
				this.Parameter2 = Math.Abs(axisY);
			}
			this.isInvoked = true;
			this._command = PtzCommand.AxisCommand;

		}
		public ActionCommand(double axisZ)
		{
			if (axisZ < 0)
			{
				this.buttonOrAxis = ButtonOrAxis.ZOOM_ADD_CONTROL;
				this.Parameter2 = axisZ;
				this.isInvoked = true;
			}else
			{
				this.buttonOrAxis = ButtonOrAxis.ZOOM_DEC_CONTROL;
				this.Parameter2 = axisZ;
				this.isInvoked = true;
			}
			this._command = PtzCommand.AxisCommand;
		}
		public ActionCommand(PtzCommand command, double parameter)
		{
			this.buttonOrAxis = ButtonOrAxis.None;
			this.isInvoked = false;
			this.command = command;
			this.Parameter = parameter;
		}

		public ActionCommand(PtzCommand command)
		{
			this.command = command;
		}

		public ActionCommand clone()
		{
			return new ActionCommand(command, buttonOrAxis, Parameter, Parameter2, isInvoked);
		}

		public PtzCommand command
		{
			get
			{
				return _command;
			}
			set
			{
				_command = value;
			}
		}

		public double Parameter
		{
			get
			{
				return _parameter;
			}
			set
			{
				_parameter = value;
			}
		}

		public ButtonOrAxis buttonOrAxis
		{
			get
			{
				return _buttonOrAxis;
			}
			set
			{
				_buttonOrAxis = value;
			}
		}

		public bool isInvoked
		{
			get
			{
				return _isInvoked;
			}
			set
			{
				_isInvoked = value;
			}
		}

		public double Parameter2 { get => _parameter2; set => _parameter2 = value; }

		public override bool Equals(object obj)
		{
			return this.command == ((ActionCommand)obj).command;
		}

		public override int GetHashCode()
		{
			return command.GetHashCode();
		}

	}
}
