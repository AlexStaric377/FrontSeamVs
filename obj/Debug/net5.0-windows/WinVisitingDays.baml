   M S B A M L   `   `   `  ����E  AFrontSeam, Version=1.0.1.41, Culture=neutral, PublicKeyToken=null%clr-namespace:FrontSeam	FrontSeam  5	   	       FrontSeam.WinVisitingDays   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   #localclr-namespace:FrontSeam  5	   	     � Title$  WinVisitingDays��5   	    � WindowState$ Normal=�6!    � AllowsTransparency$
 True��66    � WindowStyle$
 None=�6P    � WindowStartupLocation$ CenterOwner=�6c    � ShowInTaskbar$
 True��5   
   $	��400��6!   $	��530��6.   $��0q�6:   $��Stretch=�6E   $��Stretch=�6c   ��5   	   Z�  � DataContext 5      (   "FrontSeam.ViewModelWinVisitingDays  5   
   .��5      �� -   5         ��Name$ 	ButtonWin��5      $��Center=�6%   $	��330��6@   $	��500��6M    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5      	   Color$	 	#FF1D1504��6   
   
BlurRadius$
 35Q�61      ShadowDepth$ 20Q�6A      Opacity$	 0.5Q�6R   6c   .+�5   
   �� -   5         ��Name$ 	BorderWin��5      ���	#FFE2E2DD���	#FFE2E2DD$��0,0,0,0q�6T    �� CornerRadius$ 15,15,15,15��6n   $	��330��6�   $	��480��6�   ._�5      � -   5        $��WinGrid��5   b   $��Center=�6   $	��Top=�60   $	��320��6H   $
��Auto��6U   ���	#FFE2E2DD � RowDefinitions 5      �� 5      $	��25* �6$   �� 5      $
��270* �6$   �� 5      $	��30* �6$   .+�5      �� -   5        $��	Zagolovok��5      $7��1Розклад прийому пацієнтів ��6*   $��0��6O   $��12(�6\   $��Black"�6j   $��Center=�6}   �� 5      -   5        $��TablFeature��5   '   $��1��6   $	��Top=�6d   $	��265��5   D   ���	#FFE2E2DD$	��460��5       $��0,0,0,0q�6!   $��Hidden=�6;   
������ $
��Auto=�5!   /   ��5   :   �� *ViewVisitingDayss+ �� SelectedItem 5      �� *SelectedVisitingDays+��5#      �� 5$      .R�5%      ��  �� Orientation$ 
Horizontal=�6)   $��5q�6B   .+�5&   "   �� $
��Wrap=�6P   $��90��6f   $��20��6q   $��14(�6}   $��Microsoft Sans Serif)�6�   $��Black"�6�   $
��Left=�6�   ��6,   ��  �� Path$ daysOfTheWeek�6�   �� 5'   "   $
��Wrap=�6M   $��90��6c   $��20��6n   $��14(�6z   $��Microsoft Sans Serif)�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $ 
dateVizita�6�   �� 5(   "   $
��Wrap=�6M   $��80��6c   $��20��6n   $��14(�6z   $��Microsoft Sans Serif)�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $ 
timeVizita�6�   �� 5)   "   $
��Wrap=�6H   $��70��6^   $��20��6i   $��14(�6u   $��Microsoft Sans Serif)�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $ onOff�6�   5+      5,      5.      �� 5/      -   5        $��
Close_Grid��5/   Y   $��20��6   $��20��6$   $��0��6/   $��Right=�6<   $��Bottom=�6l   $C��=pack://application:,,,/FrontSeam;component/Images/xrestik.png��50       � Cursor$
 Hand��51      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 52      +  %Microsoft.Xaml.Behaviors.EventTrigger  53         	EventName$ MouseLeftButtonDown��6)   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 54      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     CommandParameter$  ��6b      Command 64   �� *CloseModelWinVisitingDays+6b   55      57      � 58      $��2��6    � ColumnDefinitions 59      �� 5:      $
��100* �6+   �� 5;      $
��100* �6+   .+�5=      �� $��2��6   $��0��6*   $��25��6:   $��70��6F   ���	#FF3E3E3B$��Bottom=�6i   ���	#FFE2E2DD$��1,1,1,1q�5>   5   $ 15,15,15,15��6O   $
 Hand��6j    5?        5@      $ MouseLeftButtonDown��6-   . 5A   "     $  ��6_    68   �� *SelectDaysVisiting+6_   5B      ._�5D      �� $��2��6    $��25��60   $��10(�6<   $��70��6K   $��Вибрати��6V   $��Center=�6h   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Arial)�6�   5E      5F      �� 5G      $��2��6   $��1��6*   $��25��6:   $��80��6F   ���	#FF3E3E3B$��Bottom=�6i   ���	#FFE2E2DD$��1,1,1,1q�5H   5   $ 15,15,15,15��6O   $
 Hand��6j    5I        5J      $ MouseLeftButtonDown��6-   . 5K   "     $  ��6f    68   �� *CloseModelWinVisitingDays+6f   5L      ._�5N      �� $��2��6    $��25��60   $��10(�6<   $��Normal$�6K   $��Courier)�6^   $��65��6t   $��Завершити��6   $��Center=�6�   $��Center=�6�   $��Center=�6�   5O      5P      5Q      5S      5T      5U      5V      