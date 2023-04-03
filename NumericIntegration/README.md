# LandauDistribution - NumericIntegration

Since the convergence is different, it is necessary to use different equations for positive and negative &lambda;.

## &lambda; &geq; 0

Since p(&lambda;) has periodicity due to sin, this is used.

![integrate px 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_1.svg)  

![integrad px](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrand_px.svg)  

If sin is evaluated here for each cycle, the point of integration can always be positive.

![integrate px 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_2.svg)  
![integrate px 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_3.svg)  

Evaluate the peak point of the integrated function. It can be seen that t &lt; 1 always.

![integrate px 4](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_4.svg)  

Evaluate the upper bound.

![integrate px 7](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_7.svg)  
![integrate px 5](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_5.svg)  
![integrate px 6](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_6.svg)  

Similarly, a lower bound is required.

![integrate px 8](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_8.svg)  

## &lambda; &lt; 0

For negative &lambda;, do a variable transformation and use an equation with small oscillations that decay rapidly.  

![integrate nx 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_nx_1.svg)  

![integrad nx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrand_nx.svg)  

When &lambda; &rarr; -&infin;, the integral J converges to 1.

![integrate nx 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_nx_3.svg)  
![integrate nx 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_nx_2.svg)  

### Reference
[W.Börsch-Supan(1961)](https://nvlpubs.nist.gov/nistpubs/jres/65B/jresv65Bn4p245_A1b.pdf)