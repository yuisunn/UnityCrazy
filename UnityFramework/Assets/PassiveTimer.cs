using System;
using System.Collections;
using UnityEngine;

public sealed class PassiveTimer
{
	public double StartTime { get { return _startTime; } }
	public double ElapsedTime { get { return _elaspsedTime; } }
	public double CurrentTime { get { return _startTime + _elaspsedTime; } }

	private readonly double _interval;
	private double _startTime;
	private double _elaspsedTime;


	public PassiveTimer(double interval)
		: this(interval, 0)
	{
	}

	public PassiveTimer(double interval, double startTime)
	{
		if(interval <= 0)
		{
			throw new ArgumentOutOfRangeException("interval");
		}

		_interval = interval;
		_startTime = startTime;
	}

	public int Update(double deltaTime)
	{
		if(deltaTime == 0)
		{
			// deltaTime = 0, no update
			return 0;
		}

		double offset = MathUtil.mod(_startTime + _elaspsedTime, _interval);
		if(deltaTime < 0) { offset = _interval - offset; }

		double destTimeAbs = Math.Abs(offset) + Math.Abs(deltaTime);
		double intervalAbs = Math.Abs(_interval);

		int hitCount = (int)(destTimeAbs / intervalAbs);

		_elaspsedTime += deltaTime;
		return hitCount;
	}

	public void Reset()
	{
		_elaspsedTime = 0;
	}
}
