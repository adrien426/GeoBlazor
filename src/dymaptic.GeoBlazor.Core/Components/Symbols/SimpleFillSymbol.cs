﻿using dymaptic.GeoBlazor.Core.Objects;
using dymaptic.GeoBlazor.Core.Serialization;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;


namespace dymaptic.GeoBlazor.Core.Components.Symbols;

/// <summary>
///     SimpleFillSymbol is used for rendering 2D polygons in either a MapView or a SceneView. It can be filled with a
///     solid color, or a pattern. In addition, the symbol can have an optional outline, which is defined by a
///     SimpleLineSymbol.
///     <a target="_blank" href="https://developers.arcgis.com/javascript/latest/api-reference/esri-symbols-SimpleFillSymbol.html">
///         ArcGIS
///         JS API
///     </a>
/// </summary>
public class SimpleFillSymbol : FillSymbol, IEquatable<SimpleFillSymbol>
{
    /// <summary>
    ///     Parameterless constructor for using as a razor component
    /// </summary>
    public SimpleFillSymbol()
    {
    }

    /// <summary>
    ///     Constructs a new SimpleFillSymbol in code with parameters
    /// </summary>
    /// <param name="outline">
    ///     The outline of the polygon.
    /// </param>
    /// <param name="color">
    ///     The color of the polygon.
    /// </param>
    /// <param name="fillStyle">
    ///     The fill style.
    /// </param>
    public SimpleFillSymbol(Outline? outline = null, MapColor? color = null, FillStyle? fillStyle = null)
    {
        Outline = outline;
        Color = color;
        FillStyle = fillStyle;
    }

    /// <summary>
    ///     Compares two <see cref="SimpleFillSymbol" />s for equality
    /// </summary>
    public static bool operator ==(SimpleFillSymbol? left, SimpleFillSymbol? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Compares two <see cref="SimpleFillSymbol" />s for inequality
    /// </summary>
    public static bool operator !=(SimpleFillSymbol? left, SimpleFillSymbol? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     The outline of the polygon.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Outline? Outline { get; set; }

    /// <summary>
    ///     The fill style.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("style")]
    [Parameter]
    public FillStyle? FillStyle { get; set; }

    /// <inheritdoc />
    public override string Type => "simple-fill";

    /// <inheritdoc />
    public bool Equals(SimpleFillSymbol? other)
    {
        if (ReferenceEquals(null, other)) return false;

        return Equals(Outline, other.Outline) && (FillStyle == other.FillStyle) && (Color == other.Color);
    }

    /// <inheritdoc />
    public override async Task RegisterChildComponent(MapComponent child)
    {
        switch (child)
        {
            case Outline outline:
                if (!outline.Equals(Outline))
                {
                    Outline = outline;
                }

                break;
            default:
                await base.RegisterChildComponent(child);

                break;
        }
    }

    /// <inheritdoc />
    public override async Task UnregisterChildComponent(MapComponent child)
    {
        switch (child)
        {
            case Outline _:
                Outline = null;

                break;
            default:
                await base.UnregisterChildComponent(child);

                break;
        }
    }

    /// <inheritdoc />
    public override void ValidateRequiredChildren()
    {
        base.ValidateRequiredChildren();
        Outline?.ValidateRequiredChildren();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != GetType()) return false;

        return Equals((SimpleFillSymbol)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Outline, FillStyle, Color);
    }

    internal override SymbolSerializationRecord ToSerializationRecord()
    {
        return new SimpleFillSymbolSerializationRecord(
            Outline?.ToSerializationRecord() as SimpleLineSymbolSerializationRecord, Color, FillStyle);
    }
}

internal record SimpleFillSymbolSerializationRecord(
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]SimpleLineSymbolSerializationRecord? Outline = null, 
    MapColor? Color = null, 
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]FillStyle? Style = null) 
    : SymbolSerializationRecord("simple-fill", Color);

/// <summary>
///     The possible fill style for the <see cref="SimpleFillSymbol" />
/// </summary>
[JsonConverter(typeof(EnumToKebabCaseStringConverter<FillStyle>))]
public enum FillStyle
{
#pragma warning disable CS1591
    BackwardDiagonal,
    Cross,
    DiagonalCross,
    ForwardDiagonal,
    Horizontal,
    None,
    Solid,
    Vertical
#pragma warning restore CS1591
}