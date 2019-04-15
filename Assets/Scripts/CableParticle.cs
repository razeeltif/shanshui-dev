using UnityEngine;
using System.Collections;

public class CableParticle
{
	#region Class member variables

	private Vector3 _position, _oldPosition, _offset;
	private Transform _boundTo = null;
	private Rigidbody _boundRigid = null;

	#endregion


	#region Properties

	public Vector3 Position {
		get { return _position; }
		set { _position = value; }
	}
		
	public Vector3 Velocity {
		get { return (_position - _oldPosition); }
	}

	#endregion


	#region Constructor

	public CableParticle(Vector3 newPosition)
	{
		_oldPosition = _position = newPosition;
	}

	#endregion


	#region Public functions

	public void UpdateVerlet(Vector3 gravityDisplacement)
	{
		if (this.IsBound())
		{
			if (_boundRigid == null) {
				this.UpdatePosition(_boundTo.position + _boundTo.right * _offset.x + _boundTo.up * _offset.y + _boundTo.forward * _offset.z);		
			}
			else
			{
				switch (_boundRigid.interpolation) 
				{
				case RigidbodyInterpolation.Interpolate:
					this.UpdatePosition(_boundRigid.position + _boundTo.right * _offset.x + _boundTo.up * _offset.y + _boundTo.forward * _offset.z + (_boundRigid.velocity * Time.fixedDeltaTime) / 2);
					break;
				case RigidbodyInterpolation.None:
				default:
					this.UpdatePosition(_boundRigid.position + _boundTo.right * _offset.x + _boundTo.up * _offset.y + _boundTo.forward * _offset.z  + _boundRigid.velocity * Time.fixedDeltaTime);
					break;
				}
			}
		}
		else 
		{
			Vector3 newPosition = this.Position + this.Velocity + gravityDisplacement;
			this.UpdatePosition(newPosition);
		}
	}

	public void UpdatePosition(Vector3 newPos) 
	{
		_oldPosition = _position;
		_position = newPos;
	}

	public void Bind(Transform to, Vector3 off)
	{
		_boundTo = to;
        _offset = off;
		_boundRigid = to.GetComponent<Rigidbody>();
		_oldPosition = _position = _boundTo.position + _boundTo.right * _offset.x + _boundTo.up * _offset.y + _boundTo.forward * _offset.z;
	}
		
	public void UnBind()
	{
		_boundTo = null;
		_boundRigid = null;
	}
		
	public bool IsFree()
	{
		return (_boundTo == null);
	}
		
	public bool IsBound()
	{
		return (_boundTo != null);
	}

	#endregion
}