# LandauDistribution - NumericIntegration

Since the convergence is different, it is necessary to use different equations for positive and negative x.

## x &geq; 0

Since p(x) has periodicity due to sin, this is used.

![integrad px](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrand_px.svg)  

![integrate px 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_1.svg)  
![integrate px 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_2.svg)  

If x is positive, the integral value is always positive.

![integrate px 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_3.svg)  

Evaluate the peak point of the integrated function. It can be seen that t &lt; 1 always.

![integrate px 4](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_4.svg)  

Evaluate the upper bound.

![integrate px 7](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_7.svg)  
![integrate px 5](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_5.svg)  
![integrate px 6](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_6.svg)  

Similarly, a lower bound is required.

![integrate px 8](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_8.svg)  

## x &lt; 0

Use an equation with small oscillations whose absolute value is not greater than 1 for negative x.  
However, the period is not fixed.  
Furthermore, for negative x, the value of the probability density function decreases rapidly,   
allowing the calculation to be terminated before the oscillations become too severe.

![integrad nx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrand_nx.svg)  

![integrate nx 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_nx_1.svg)  