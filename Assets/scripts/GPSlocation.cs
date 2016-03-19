using UnityEngine;
using System;

public class GPSlocation
{
	public double latitude;
	public double longitude;
    public double horizontalAccuracy;
    public LocationInfo? locinfo;

	public GPSlocation(double x, double y, double ha)
	{
		latitude = x;
		longitude = y;
        horizontalAccuracy = ha;
        locinfo = null;
	}

    public GPSlocation(LocationInfo loc) : this(loc.latitude, loc.longitude, loc.horizontalAccuracy) {
        locinfo = loc;
    }

    public static double Distance(GPSlocation a, GPSlocation b) 
	{
		double theta = a.longitude - b.longitude;

		double dist = Math.Sin(Mathf.Deg2Rad * a.latitude) * Math.Sin(Mathf.Deg2Rad * b.latitude) + 
					  Math.Cos(Mathf.Deg2Rad * a.latitude) * Math.Cos(Mathf.Deg2Rad * b.latitude) * Math.Cos( Mathf.Deg2Rad * theta);
		dist = Math.Acos(dist);
		dist = Mathf.Rad2Deg * dist;
		dist = dist * 60 * 1.1515;

		dist = dist * 1609.344; //m
//		   dist = dist * 1.609344; //km
//		   dist = dist * 0.8684;   //miles
		return dist;
		//I don't know why there is an offset of 90 degrees. Angle is counterclockwise
	}

	public static double Direction(GPSlocation a, GPSlocation b)
	{
		double dLatitude  = b.latitude - a.latitude;
		double dLongitude = b.longitude - a.longitude;

		double angle = System.Math.Atan2( dLatitude, System.Math.Cos(Mathf.PI/180*a.latitude) * dLongitude);

		//angle = angle * Mathf.Rad2Deg;

		return (angle -90); // convention: 0 degrees when it's vertical, positive counterclockwise
	}

    public static explicit operator Vector3 (GPSlocation loc)
    {
        return new Vector3((float)loc.latitude, 0, (float)loc.longitude);
    }
}
