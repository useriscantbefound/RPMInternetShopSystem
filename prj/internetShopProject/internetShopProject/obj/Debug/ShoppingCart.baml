   M S B A M L   `   `   `  ����N  JinternetShopProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:internetShopProjectinternetShopProject  5   	   &     internetShopProject.ShoppingCart   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:internetShopProject     5   	   -   5          � Title$  Shopping Cart��5   	   $	��450��6   $	��500��6,    � 
ResizeMode$ CanMinimize=�68   .��5	      �  � RowDefinitions 5
   
   �� 5      $��60 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   ���#5d1d99.+�5      ��    shoppingCartTextBlock#��[  6   $��Shopping Cart��6G   �� 5      -   5          shoppingCartButton#��[ 5      $��Order��6A   6Q   �� 5      -   5        #��[ 5      $��Clear��6A   6Q   5      �� 5   
   $ 
Horizontal=�6   $��1��6.   ���#461673.+�5      �� -   5        $��cartListViewForProducts��5      $	��500��6c   
������ $��0q�6�   ��65   �� *cartProductCollection+��5      �� 5      .R�5      �� .+�5      �� $��cartProductNameTextBlock��6(   
�������$��16(�6[   �� 5   "   $�	Product: ��6&    �� 5   "   �6&   �� *product_Name+5      �� 5!      $��cartProductDescTextBlock��6(   
�������$��16(�6[   �� 5"   "   $�Description: ��6&    �� 5#   "   �6&   �� *product_Description+5$      �� 5&      $��cartProductCountTextBlock��6(   
�������$��16(�6\   �� 5'   "   $�Count: ��6&    �� 5(   "   �6&   �� *product_Count+5)      �� 5+      $��cartProductPriceTextBlock��6(   
�������$��16(�6\   �� 5,   "   $�Price: ��6&    �� 5-   "   �6&   �� *product_Price+5.      5/      50      52      53      �� 55   
   $��2��6   ���#5d1d996"   56      57      