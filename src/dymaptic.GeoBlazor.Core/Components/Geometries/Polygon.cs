﻿using dymaptic.GeoBlazor.Core.Objects;
using Microsoft.AspNetCore.Components;

namespace dymaptic.GeoBlazor.Core.Components.Geometries;

/// <summary>
///     A polygon contains an array of rings and a spatialReference. Each ring is represented as an array of points. The first and last points of a ring must be the same. A polygon also has boolean-valued hasM and hasZ fields.
///     <a target="_blank" href="https://developers.arcgis.com/javascript/latest/api-reference/esri-geometry-Polygon.html">ArcGIS JS API</a>
/// </summary>
public class Polygon : Geometry, IEquatable<Polygon>
{
    /// <summary>
    ///     Parameterless constructor for use as a razor component
    /// </summary>
    public Polygon()
    {
    }

    /// <summary>
    ///     Creates a new polygon in code with parameters
    /// </summary>
    /// <param name="rings">
    ///     An array of <see cref="MapPath"/> rings.
    /// </param>
    /// <param name="spatialReference">
    ///     The <see cref="SpatialReference"/> of the geometry.
    /// </param>
    /// <param name="extent">
    ///     The <see cref="Extent"/> of the geometry.
    /// </param>
    public Polygon(MapPath[] rings, SpatialReference? spatialReference = null, Extent? extent = null)
    {
#pragma warning disable BL0005
        Rings = rings;
        SpatialReference = spatialReference;
        Extent = extent;
#pragma warning restore BL0005
    }
    
    /// <summary>
    ///     An array of <see cref="MapPath"/> rings.
    /// </summary>
    [Parameter]
    public MapPath[] Rings
    {
        get => _rings;
        set
        {
            if (!_rings.SequenceEqual(value, MapPathEqualityComparer.Instance))
            {
                _rings = value.Select(p => p.DeepCopy()).ToArray();
                Task.Run(UpdateComponent);
            }
        }
    }

    /// <inheritdoc />
    public override string Type => "polygon";

    private MapPath[] _rings = Array.Empty<MapPath>();

    public bool Equals(Polygon? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return _rings.Equals(other._rings);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return Equals((Polygon)obj);
    }

    public override int GetHashCode()
    {
        return _rings.GetHashCode();
    }

    public static bool operator ==(Polygon? left, Polygon? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Polygon? left, Polygon? right)
    {
        return !Equals(left, right);
    }
}