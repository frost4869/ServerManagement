   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   M IServerManagement, Version=11.7.17.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:ServerManagement.ViewServerManagement.View 5   	   '   !ServerManagement.View.LoginWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   5local#clr-namespace:ServerManagement.View     5   	     � Title$  LoginWindow��5	   	   $	��300��6   $	��600��6*   .��5
      � $��Center=�6    � RowDefinitions 5   
   �� 5      $��50 �6   6*   �� 5      $��50 �6   6*   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5   
   �� -   5         ��Name$ txtUsername��5      $��0��6A   $	��400��6N   $��Center=�6Z   *   $MahApps.Metro.Controls.TextBoxHelper   UseFloatingWatermark .    	Watermark$ Username��5      ��5   '   �� *
Username+.��5   >    J� InputBindings 5      �� 5      
 �� Key$ Return��6s    �� Command 6   �� *LoginCommand+ �� CommandParameter 6>   �� 	 �� ElementName$	 passwordBox��6s   .��5   %   5      G� 5   
   -   5        
 G�Name$
 passwordBox��5      $��1��6+   $	��400��68   $��Center=�6D    . $ Password��5       "  MetroButtonRevealedPasswordBox#��[  5       5      �� 5       $ Return��6s    6   �� *LoginCommand+ 6>   �� $	 passwordBox��6s   5"      �� 5#   
   $��2��6,   $��Center=�5$      
���  ��$
��Wrap=�6@   $��Center=�6T   $��13(�5%      $��DemiBold"�6"   ��5#      �� *Status+5%   "   �� 5'   
   $��3��6   $��login��6     SquareButtonStyle#���  6.   $��Center=�5*      $	��100��5+      $��13(�6   $��0 30 0 0q�6+   ��5(      �� *LoginCommand+��5)      �� $	 passwordBox��5+   +   �� 5,   
   $��4��6   $��Center=�5-      $��0 10 0 0q�5.      $��13(�5/      �� 50      -   5        .�50   0   Check for Update6B   51      52      53      