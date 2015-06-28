﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ludiq.Controls
{
	/// <summary>
	/// Utility class to display complex editor popups.
	/// </summary>
	public static class PopupGUI<T>
	{
		public delegate void PopupCallback(T value);

		/// <summary>
		/// Render an editor popup and return the newly selected option.
		/// </summary>
		/// <param name="position">The position of the control.</param>
		/// <param name="callback">The function called when a value is selected.</param>
		/// <param name="options">The list of available options.</param>
		/// <param name="selectedOption">The selected option, or null for none.</param>
		/// <param name="noneOption">The option for "no selection", or null for none.</param>
		/// <param name="hasMultipleDifferentValues">Whether the content has multiple different values.</param>
		public static void Render
		(
			Rect position,
			PopupCallback callback,
			IEnumerable<PopupOption<T>> options,
			PopupOption<T> selectedOption,
			PopupOption<T> noneOption,
			bool hasMultipleDifferentValues
		)
		{
			bool hasOptions = options != null && options.Any();

			string label;

			if (hasMultipleDifferentValues)
			{
				label = "—";
			}
			else if (selectedOption == null)
			{
				if (noneOption != null)
				{
					label = noneOption.label;
				}
				else
				{
					label = string.Empty;
				}
			}
			else
			{
				label = selectedOption.label;
			}

			if (GUI.Button(position, label, EditorStyles.popup))
			{
				GenericMenu menu = new GenericMenu();
				GenericMenu.MenuFunction2 menuCallback = (o) => callback((T)o);

				if (noneOption != null)
				{
					menu.AddItem(new GUIContent(noneOption.label), false, menuCallback, noneOption.value);
				}

				if (noneOption != null && hasOptions)
				{
					menu.AddSeparator("");
				}

				if (hasOptions)
				{
					foreach (var option in options)
					{
						menu.AddItem(new GUIContent(option.label), false, menuCallback, option.value);
					}
				}

				Vector2 menuPosition = new Vector2(position.xMin, position.yMax);

				menu.DropDown(new Rect(menuPosition, Vector2.zero));
			}
			else if (selectedOption != null && !options.Select(o => o.value).Contains(selectedOption.value))
			{
				// Selected option isn't in range

				if (noneOption != null)
				{
					callback(noneOption.value);
				}
				else
				{
					callback(default(T));
				}
			}
		}
	}
}