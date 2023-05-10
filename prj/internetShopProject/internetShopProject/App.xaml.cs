using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Xml.Linq;
using System.Collections.Generic;

namespace internetShopProject
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>

	public partial class App : Application
	{
		// Создание "контекста" для обращения к элементам базы данных (таблицам и столбцам в них)
		public static mainEntities.internetShop_DBEntities6 Context
		{ get; } = new mainEntities.internetShop_DBEntities6();
    }
}