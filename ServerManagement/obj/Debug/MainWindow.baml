   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   M IServerManagement, Version=11.7.17.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ServerManagementServerManagement 5   	   =#clr-namespace:ServerManagement.ViewServerManagement.View 5   	   !   ServerManagement.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   0localclr-namespace:ServerManagement     5   	   5views#clr-namespace:ServerManagement.View     5   	   R NMahApps.Metro.IconPacks, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5	   	     � Title$  
MainWindow��5   	   $	��600��6   $
��1000��6)   $	��700��66    � 	Resources 5         AppThemeMenuItemStyle  MetroMenuItem �� BasedOn �� 
TargetType �� Property �� Value �� Mode��%�   &	        7  �� 5   
   8   " m�5      .�5      ��  �@m�Command 6(   �� *ChangeAccentCommand+�� 5       � �� 6'   �� *Name+$ OneWay=�5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   ]� $��0��6   .+�5      o� $��5q�6   $
��Left=�5      $��Stretch=�5       $
��Left=�5!       5"        BoolToVisConverter��%   &	       �� 5#      63   5$      .@�5%      m� $
��Edit��6   .@�5&      m� -   5        $��
New Server��5&      63   m� 5'      -   5        $��Import from Excel��5'      6:   m� 5(      -   5        $��Export to Excel��5(      68   5)      m� 5*      $��Account��6   y�6,   ��  �� Path$ IsAdmin�	 �� 	Converter#	 [ .@�5+      m� -   5        $��Manage Account��5+      6P   m� 5,      -   5        $��Create New Account��5,      6T   5-      m� 5.      -   5        $��Check for Update��5.      6L   5/      � 50      $��Right=�6   $��5q�6*   $��Right=�65   $��Stretch=�6Q     
BlackBrush+   %MahApps.Metro.Controls.ControlsHelper
   MouseOverBorderBrush#
 �  51      .+�52      o�  53      ��%   &	       �� 54      67   55      .@�56      m� $	��150��6    $��Center=�6,   $��Center=�6I   ��57      ]� 58      .+�59   "   ,  &MahApps.Metro.IconPacks.PackIconModern  $��20��6;   $
��Left=�6F   $��20��5:   /   $��Center=�6;   �  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconModernKind, MahApps.Metro.IconPacks, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null]]   Kind$
 User=�5;   /   �� 5<   "   $��15 0 0 0q�6[   $��Right=�6m    �� ContentStringFormat$ {0} ⏷��6�   ��6(   �� *AuthenticatedUser+$ OneWay=�6�   5=      .@�5?      m� -   5        $��Change Password��5?   #   y�6Y   �� $ IsAdmin�#	 [ m� 5@      $��Logout ��6#    m� Command 64   �� *LogoutCommand+5A      5B      5C      5D      =   7MahApps.Metro.Controls.MetroAnimatedSingleRowTabControl  5F   
   -   5          Name$ MainTabControl��5H      $��2��5I      -   'MahApps.Metro.Controls.TabControlHelper   
Underlined$ TabPanel=�5J      5K      �� 5L   
   $��3��6   .@�5M      �� .��6   Server Management Tool64   �� 5N        MetroStatusBarSeparator#��[ 6   �� 5O      -	   5         ��Name$ progressText��5O      $y�	Collapsed=�62   $
��Bold"�6I   .��6[   
Exporting:6g   �� 5P      .��5Q      -   'MahApps.Metro.Controls.MetroProgressBar  -
   5          Name$ ExportPB��5Q   C   $y�	Collapsed=�6,   $��15��6U   $	��500��6a    � Minimum$ 0Q�6m    � Maximum$	 100Q�6y   6�   5R      5S      5T      5U      